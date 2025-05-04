using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace MH.Portal
{

    public class Portal : MonoBehaviour
    {
        #region -------------------- Inspectors -------------------

        [SerializeField] private Portal linkedPortal;
        [SerializeField] private MeshRenderer screen;

        
        [SerializeField] private Camera playerCamera;
        private Camera _portalCamera;
        private RenderTexture _viewTexture;
        private MeshFilter _screenMeshFilter;
        List<PortalTraveller> _trackedTravellers = new();
        
        [Header ("Advanced Settings")]
        public float nearClipOffset = 0.05f;
        public float nearClipLimit = 0.2f;

        [SerializeField] private int recursionLimit = 5;
        #endregion

        #region ---------------------- Untiy Methods --------------------

        void Awake()
        {
            // playerCamera = Camera.main;
            _portalCamera = GetComponentInChildren<Camera>();
            _portalCamera.enabled = false;

            _screenMeshFilter = screen.GetComponent<MeshFilter>();
            
            playerCamera = Camera.main;
        }

        private void Update()
        {
            HandleTravellers(); // update pos, rot of traveller's clone & try teleport traveller
            // UpdateParamsForAllTravellers(); // update params for traveller's shader (use for slice process)
            
            Render(); // move, rotate portal cam -> call cam.Render() ->  update image on linked portal screen
            
        }

        private void LateUpdate()
        {
            UpdateParamsForAllTravellers(); // update params for traveller's shader  (use for slice process)
            ProtectScreenFromClipping(playerCamera.transform.position); // move screen with a small dst to avoid playerCam collision with screen.
        }

        private void OnTriggerEnter(Collider other)
        {
            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller)
            {
                OnTravellerEnter(traveller);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller && _trackedTravellers.Contains(traveller))
            {
                traveller.ExitPortalThreshold();
                _trackedTravellers.Remove(traveller);
            }
        }

        #endregion

        #region -------------------- Public Methods ------------------
         
        // Manually render the camera attached to this portal
        // Called after PrePortalRender, and before PostPortalRender
        public void Render() {

            // Skip rendering the view from this portal if player is not looking at the linked portal
            if (!CameraExtension.VisibleFromCamera (linkedPortal.screen, playerCamera)) {
                return;
            }

            TryCreateViewTexture ();

            var localToWorldMatrix = playerCamera.transform.localToWorldMatrix;
            var renderPositions = new Vector3[recursionLimit];
            var renderRotations = new Quaternion[recursionLimit];

            int startIndex = 0;
            _portalCamera.projectionMatrix = playerCamera.projectionMatrix;
            for (int i = 0; i < recursionLimit; i++) {
                if (i > 0) {
                    // No need for recursive rendering if linked portal is not visible through this portal
                    if (!CameraExtension.BoundsOverlap (_screenMeshFilter, linkedPortal._screenMeshFilter, _portalCamera)) {
                        break;
                    }
                }
                localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
                int renderOrderIndex = recursionLimit - i - 1;
                renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn (3);
                renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

                _portalCamera.transform.SetPositionAndRotation (renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
                startIndex = renderOrderIndex;
            }

            // Hide screen so that camera can see through portal
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            // linkedPortal.screen.material.SetInt ("displayMask", 0);

            for (int i = startIndex; i < recursionLimit; i++) {
                _portalCamera.transform.SetPositionAndRotation (renderPositions[i], renderRotations[i]);
                SetNearClipPlane ();
                HandleClipping ();
                _portalCamera.Render ();

                if (i == startIndex) {
                    // linkedPortal.screen.material.SetInt ("displayMask", 1);
                }
            }

            // Unhide objects hidden at start of render
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        public Vector3 GetRelativePortalPosition(Vector3 pos)
        {
            // Get the Transform of portal A (this object's transform)
            Transform portalA = transform;

            // Get the Transform of portal B (the linked portal's transform)
            Transform portalB = linkedPortal.transform;

            // Calculate the relative position of the point 'pos' with respect to portal A in local space
            Vector3 localPos = portalA.InverseTransformPoint(pos);

            // Convert that local position into world space relative to portal B
            Vector3 worldPos = portalB.TransformPoint(localPos);

            // Calculate the rotational difference between the two portals
            Quaternion rotationDifference = portalB.rotation * Quaternion.Inverse(portalA.rotation);

            // Rotate the resulting position based on the rotational difference between the two portals
            Vector3 offset = worldPos - portalB.position;
            Vector3 rotatedOffset = rotationDifference * offset;

            // Return the final position after applying the offset to portal B's position
            return portalB.position + rotatedOffset;
        }


        #endregion

        #region ------------------- Private Methods -----------------

        void TryCreateViewTexture()
        {
            if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
            {
                if(_viewTexture != null) _viewTexture.Release();

                //creates a new RenderTexture object with the current screen width, height, and a depth buffer of 24 bits.
                //This RenderTexture is used to render the view from the portal camera.
                _viewTexture = new RenderTexture(Screen.width, Screen.height, 24);
                    
                // set the texture to the camera
                _portalCamera.targetTexture = _viewTexture;
                
                // set the texture to the screen of linked portal
                linkedPortal.screen.material.SetTexture("_MainTex", _viewTexture);
            }
            
        }
 
        
        // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
        // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
        void SetNearClipPlane () {
            // Learning resource:
            // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            Transform clipPlane = transform;
            int dot = System.Math.Sign (Vector3.Dot (clipPlane.forward, transform.position - _portalCamera.transform.position));

            Vector3 camSpacePos = _portalCamera.worldToCameraMatrix.MultiplyPoint (clipPlane.position);
            Vector3 camSpaceNormal = _portalCamera.worldToCameraMatrix.MultiplyVector (clipPlane.forward) * dot;
            float camSpaceDst = -Vector3.Dot (camSpacePos, camSpaceNormal) + nearClipOffset;

            // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
            if (Mathf.Abs (camSpaceDst) > nearClipLimit) {
                Vector4 clipPlaneCameraSpace = new Vector4 (camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

                // Update projection based on new clip plane
                // Calculate matrix with player cam so that player camera settings (fov, etc) are used
                _portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix (clipPlaneCameraSpace);
            } else {
                _portalCamera.projectionMatrix = playerCamera.projectionMatrix;
            }
        }


        #region ------ Teleportation------

        void OnTravellerEnter(PortalTraveller traveller)
        {
            if (!_trackedTravellers.Contains(traveller))
            {
                traveller.EnterPortalThreshold();
                traveller.PreviousOffsetFromPortal = traveller.transform.position - transform.position;
                _trackedTravellers.Add(traveller);
            }
        }
        
        // sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
        /*float ProtectScreenFromClipping()
        {
            float halfHeight = playerCamera.nearClipPlane * Mathf.Tan(playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfWidth = playerCamera.aspect * halfHeight;
            float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCamera.nearClipPlane).magnitude;
            
            Transform screenT = screen.transform;
            bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - playerCamera.transform.position) > 0;
            
            // screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, dstToNearClipPlaneCorner);
            // screenT.localPosition = Vector3.forward * dstToNearClipPlaneCorner * (camFacingSameDirAsPortal ? 0.5f : -0.5f);
            
            screenT.localScale = new Vector3(dstToNearClipPlaneCorner, screenT.localScale.y, screenT.localScale.z);
            screenT.localPosition = Vector3.right * dstToNearClipPlaneCorner * (camFacingSameDirAsPortal ? -0.5f : 0.5f);
        }*/
        
        // Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
        float ProtectScreenFromClipping (Vector3 viewPoint) {
            float halfHeight = playerCamera.nearClipPlane * Mathf.Tan (playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * playerCamera.aspect;
            float dstToNearClipPlaneCorner = new Vector3 (halfWidth, halfHeight, playerCamera.nearClipPlane).magnitude;
            float screenThickness = dstToNearClipPlaneCorner;

            Transform screenT = screen.transform;
            bool camFacingSameDirAsPortal = Vector3.Dot (transform.forward, transform.position - viewPoint) > 0;
            // screenT.localScale = new Vector3 (screenT.localScale.x, screenT.localScale.y, screenThickness);
            // screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
            
            screenT.localScale = new Vector3(screenThickness, screenT.localScale.y, screenT.localScale.z);
            screenT.localPosition = Vector3.right * screenThickness * (camFacingSameDirAsPortal ? -0.5f : 0.5f);
            return screenThickness;
        }

        void TryTeleportTravellers()
        {
            for (int i=0; i<_trackedTravellers.Count; i++)
            {
                PortalTraveller traveller = _trackedTravellers[i];
                Transform travellerT = traveller.transform;
                
                Vector3 offsetFromPortal = travellerT.position - transform.position;
                int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.PreviousOffsetFromPortal, transform.forward));
                
                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                    traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);
                    
                    // Can not rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    linkedPortal.OnTravellerEnter(traveller);
                    _trackedTravellers.RemoveAt(i);
                    i--;
                }
                else
                {
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }  
            }
        }

        #endregion


        #region -------- Slice --------

        void HandleClipping () {
            // There are two main graphical issues when slicing travellers
            // 1. Tiny sliver of mesh drawn on backside of portal
            //    Ideally the oblique clip plane would sort this out, but even with 0 offset, tiny sliver still visible
            // 2. Tiny seam between the sliced mesh, and the rest of the model drawn onto the portal screen
            // This function tries to address these issues by modifying the slice parameters when rendering the view from the portal
            // Would be great if this could be fixed more elegantly, but this is the best I can figure out for now
            const float hideDst = -1000;
            const float showDst = 1000;
            float screenThickness = linkedPortal.ProtectScreenFromClipping (_portalCamera.transform.position);
            
            Vector3 portalCamPos = _portalCamera.transform.position;
            
            foreach (var traveller in _trackedTravellers) {
                if (SameSideOfPortal (traveller.transform.position, portalCamPos)) {
                    // Addresses issue 1
                    traveller.SetSliceOffsetDst (hideDst, false);
                } else {
                    // Addresses issue 2
                    traveller.SetSliceOffsetDst (showDst, false);
                }

                // Ensure clone is properly sliced, in case it's visible through this portal:
                int cloneSideOfLinkedPortal = -SideOfPortal (traveller.transform.position);
                bool camSameSideAsClone = linkedPortal.SideOfPortal (portalCamPos) == cloneSideOfLinkedPortal;
                if (camSameSideAsClone) {
                    traveller.SetSliceOffsetDst (screenThickness, true);
                } else {
                    traveller.SetSliceOffsetDst (-screenThickness, true);
                }
            }

            var offsetFromPortalToCam = portalCamPos - transform.position;
            foreach (var linkedTraveller in linkedPortal._trackedTravellers) {
                var travellerPos = linkedTraveller.graphicsObject.transform.position;
                var clonePos = linkedTraveller.graphicsClone.transform.position;
                // Handle clone of linked portal coming through this portal:
                bool cloneOnSameSideAsCam = linkedPortal.SideOfPortal (travellerPos) != SideOfPortal (portalCamPos);
                if (cloneOnSameSideAsCam) {
                    // Addresses issue 1
                    linkedTraveller.SetSliceOffsetDst (hideDst, true);
                } else {
                    // Addresses issue 2
                    linkedTraveller.SetSliceOffsetDst (showDst, true);
                }

                // Ensure traveller of linked portal is properly sliced, in case it's visible through this portal:
                bool camSameSideAsTraveller = linkedPortal.SameSideOfPortal (linkedTraveller.transform.position, portalCamPos);
                if (camSameSideAsTraveller) {
                    linkedTraveller.SetSliceOffsetDst (screenThickness, false);
                } else {
                    linkedTraveller.SetSliceOffsetDst (-screenThickness, false);
                }
            }
        }
        
        private void UpdateParamsForAllTravellers()
        {
            foreach (var traveller in _trackedTravellers)
            {
                UpdateSliceParams(traveller);
            }
        }
        
        void UpdateSliceParams (PortalTraveller traveller) {
            // Calculate slice normal
            int side = SideOfPortal (traveller.transform.position);
            Vector3 sliceNormal = transform.forward * -side;
            Vector3 cloneSliceNormal = linkedPortal.transform.forward * side;

            // Calculate slice centre
            Vector3 slicePos = transform.position;
            Vector3 cloneSlicePos = linkedPortal.transform.position;

            // Adjust slice offset so that when player standing on other side of portal to the object, the slice doesn't clip through
            float sliceOffsetDst = 0;
            float cloneSliceOffsetDst = 0;
            // float screenThickness = screen.transform.localScale.z;
            float screenThickness = screen.transform.localScale.x;

            bool playerSameSideAsTraveller = SameSideOfPortal (playerCamera.transform.position, traveller.transform.position);
            if (!playerSameSideAsTraveller) {
                sliceOffsetDst = -screenThickness;
            }
            bool playerSameSideAsCloneAppearing = side != linkedPortal.SideOfPortal (playerCamera.transform.position);
            if (!playerSameSideAsCloneAppearing) {
                cloneSliceOffsetDst = -screenThickness;
            }

            // Apply parameters
            for (int i = 0; i < traveller.originalMaterials.Length; i++) {
                traveller.originalMaterials[i].SetVector ("_SliceCenter", slicePos);
                traveller.originalMaterials[i].SetVector ("_SliceNormal", sliceNormal);
                traveller.originalMaterials[i].SetFloat ("_SliceOffsetDst", sliceOffsetDst);

                traveller.cloneMaterials[i].SetVector ("_SliceCenter", cloneSlicePos);
                traveller.cloneMaterials[i].SetVector ("_SliceNormal", cloneSliceNormal);
                traveller.cloneMaterials[i].SetFloat ("_SliceOffsetDst", cloneSliceOffsetDst);

            }

        }

        // set pos and rot for clone of traveller
        // check if traveller cross portal -> teleport this traveller
        void HandleTravellers () {

            for (int i = 0; i < _trackedTravellers.Count; i++) {
                PortalTraveller traveller = _trackedTravellers[i];
                Transform travellerT = traveller.transform;
                var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;

                Vector3 offsetFromPortal = travellerT.position - transform.position;
                int portalSide = System.Math.Sign (Vector3.Dot (offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign (Vector3.Dot (traveller.PreviousOffsetFromPortal, transform.forward));
                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld) {
                    var positionOld = travellerT.position;
                    var rotOld = travellerT.rotation;
                    traveller.Teleport (transform, linkedPortal.transform, m.GetColumn (3), m.rotation);
                    traveller.graphicsClone.transform.SetPositionAndRotation (positionOld, rotOld);
                    
                    // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    linkedPortal.OnTravellerEnter(traveller);
                    _trackedTravellers.RemoveAt (i);
                    i--;

                } else {
                    traveller.graphicsClone.transform.SetPositionAndRotation (m.GetColumn (3), m.rotation);
                    //UpdateSliceParams (traveller);
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }
            }
        }
        
        #endregion
        
        int SideOfPortal (Vector3 pos) {
            return System.Math.Sign (Vector3.Dot (pos - transform.position, transform.forward));
        }
        
        bool SameSideOfPortal (Vector3 posA, Vector3 posB) {
            return SideOfPortal (posA) == SideOfPortal (posB);
        }

        #endregion

    }

}

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

        }

        private void Update()
        {
            for (int i = 0; i < _trackedTravellers.Count; i++)
            {
                UpdateSliceParams(_trackedTravellers[i]);
            }
            
            Render();
            ProtectScreenFromClipping();
        }

        private void LateUpdate()
        {
            TryTeleportTravellers();
            for (int i = 0; i < _trackedTravellers.Count; i++)
            {
                UpdateSliceParams(_trackedTravellers[i]);
            }
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

        // used to move the portal camera to the correct position and render the view from the linked portal
        // if the linked portal is not visible from the player camera, the screen of the portal will be disabled
        public void Render()
        {
            if ( !VisibleFromCamera(linkedPortal.screen, playerCamera) ) 
            {
                return;
            }
            
            screen.enabled = false;
            CheckToCreateViewTexture();
            
            Matrix4x4 localToWorldMatrix = playerCamera.transform.localToWorldMatrix;
            Matrix4x4[] matrices = new Matrix4x4[recursionLimit];
            for (int i=0; i < recursionLimit; i++)
            {
                localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
                matrices[recursionLimit - i - 1] = localToWorldMatrix;
            }
            
            // make portal cam pos and rotation the same relative to this portal as player cam relative to linked portal
            // var m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
            // _portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
            
      
            for (int i=0; i < recursionLimit; i++)
            {
                // move and rotate cam 
                _portalCamera.transform.SetPositionAndRotation(matrices[i].GetColumn(3), matrices[i].rotation);
                SetNearClipPlane(); // set near plane of cam to no see stuff which ahead portal
                _portalCamera.Render(); // call render of portal camera,new layer will blend old layer
            }
            
            screen.enabled = true;
        }

        #endregion

        #region ------------------- Private Methods -----------------

        void CheckToCreateViewTexture()
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
 
        // used to determine if a given renderer is visible from a specified camera.
        // This method is useful for optimizing rendering by checking if an object is within the camera's view frustum.
        bool VisibleFromCamera(Renderer renderer, Camera camera)
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera); 
            return GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
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
        void ProtectScreenFromClipping()
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
            float screenThickness = screen.transform.localScale.z;

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

                traveller.cloneMaterials[i].SetVector ("_SliceCentre", cloneSlicePos);
                traveller.cloneMaterials[i].SetVector ("_SliceNormal", cloneSliceNormal);
                traveller.cloneMaterials[i].SetFloat ("_SliceOffsetDst", cloneSliceOffsetDst);

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

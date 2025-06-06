using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MH.GamePlay
{
    public class PolaroidManager : MonoBehaviour
    {
        #region ------------ Fields --------------

        public FirstPersonController movementCtrl;
        public GameObject camAim_Cinema;
        [Space]
        public Camera playerCamara;
        public Camera polaroidCamera;
        public Camera bgCamera;
        public RenderTexture bgRenderTexture;
        [Space]
        public GameObject polaroidObject; 
        public GameObject polaroidView;
        public GameObject photo;
        public MeshFilter bgQuadPrefab;

        [Header(" ------ Holders ----------")]
        public Transform canCopyRoot;
        public Transform canRemoveRoot;
        public Transform polaroidHolder;
        public Transform polaroidAimPoint;
        public Transform photoHolder;
        public Transform photoAimPoint;
        [FormerlySerializedAs("cuttedObjectHolder")] public Transform cuttedContainer;

        [Header("--------------- Debug ---------------")]
        public EPolaroidState polaroidState;
        
        #endregion

        #region --------------- Properties ---------------

        private float _stateTimer;
        [SerializeField] private PolaroidState _currentState;
        
        private PolaroidStateMachine _polaroidStateMachine;
        
        #endregion

        #region ------------ Unity Methods ------------

        private void Start()
        {
            _polaroidStateMachine = new PolaroidStateMachine(this);
            
            //EnterHoldPolaroidState();
        }

        public void Update()
        {
            _polaroidStateMachine.Update();
            polaroidState = _polaroidStateMachine.CurrentKey;
            return;
            
            RunningAimPhotoState();
            RunningHoldPhotoState();
            RunningAimPolaroidState();
            RunningHoldPolaroidState();
        }

        #endregion
        
        
        
        
        #region --------- Public Methods -----------

        public void CopyAndPasteSpaceInPolaroidCamView()
        {
            // get cutable objects
            List<MeshRenderer> cutableMesh = canCopyRoot.GetComponentsInChildren<MeshRenderer>().ToList();
            List<GameObject> cutableObjects = cutableMesh.Select(mesh => mesh.gameObject).ToList();
            
            // get places which bounds the camera view
            Plane[] planes = ViewFinderExtension.GetCameraViewPlanes(polaroidCamera);
            
            // init list of cutted objects
            List<MeshData> cuttedObjectData = new ();
            
            // fill the list of cutted objects from cutable Object in scene
            for (int i=0; i < cutableObjects.Count; ++i)
            {
                MeshData meshData = new MeshData();
                meshData.Mesh = cutableObjects[i].GetComponent<MeshFilter>().mesh;
                meshData.Origin = cutableObjects[i].transform;
                cuttedObjectData.Add(meshData);
            }
            
            List<Vector3> intersectionPoints = new List<Vector3>();
            
            // generate new meshes by cutting the original meshes
            // check one by one the camera planes
            for (int i=0; i < planes.Length; ++i)
            {
                // for each plane, cut the objects
                for (int j=0; j < cuttedObjectData.Count; ++j)
                {
                    Mesh newMesh = MeshCutter.GenerateMesh(cuttedObjectData[j].Mesh, cuttedObjectData[j].Origin, planes[i], false, out intersectionPoints);
                    cuttedObjectData[j].Mesh = newMesh;
                }
            }
            
            for (int i=0; i < cuttedObjectData.Count; ++i)
            {
                GameObject newObject = MeshCutter.CreateMeshObject(cuttedObjectData[i].Origin.gameObject, cuttedObjectData[i].Mesh,cuttedObjectData[i].Origin.gameObject.name + "_Part" );
                newObject.transform.SetParent(cuttedContainer);
            }

            BuildBackgroundQuad();
        }

        public void ReplaceParentForCuttedOjects()
        {
            // get cutable objects
            List<MeshRenderer> cuttedMeshs = cuttedContainer.GetComponentsInChildren<MeshRenderer>(true).ToList();
            List<GameObject> cuttedObjects = cuttedMeshs.Select(mesh => mesh.gameObject).ToList();
            foreach (var cuttedObject in cuttedObjects)
            {
                cuttedObject.transform.SetParent(canCopyRoot);
                cuttedObject.AddComponent<MeshCollider>();
            }
        }
        
        public void CutAndRemoveSpace()
        {
            // get cutable objects
            List<MeshRenderer> cutableMesh = canRemoveRoot.GetComponentsInChildren<MeshRenderer>().ToList();
            List<GameObject> cutableObjects = cutableMesh.Select(mesh => mesh.gameObject).ToList();
            List<GameObject> oldObjects = cutableObjects.ToList();
            
            // get places which bounds the camera view
            Plane[] planes = ViewFinderExtension.GetCameraViewPlanes(playerCamara);
            
            // init list of cutted objects
            List<MeshData> cuttedObjectData = new List<MeshData>();
            List<MeshData> willGenerateData = new List<MeshData>();
            
            // fill the list of cutted objects from cutable Object in scene
            for (int i=0; i < cutableObjects.Count; ++i)
            {
                MeshData meshData = new MeshData();
                meshData.Mesh = cutableObjects[i].GetComponent<MeshFilter>().mesh;
                meshData.Origin = cutableObjects[i].transform;
                cuttedObjectData.Add(meshData);
            }
            
            List<Vector3> intersectionPoints = new List<Vector3>();
            
            for (int i=0; i < planes.Length; ++i)
            {
                for (int j=0; j < cuttedObjectData.Count; ++j)
                {
                    Mesh willShowMesh = MeshCutter.GenerateMesh(cuttedObjectData[j].Mesh, cuttedObjectData[j].Origin, planes[i], true, out intersectionPoints);
                    MeshData willShowData = new MeshData();
                    willShowData.Mesh = willShowMesh;
                    willShowData.Origin = cuttedObjectData[j].Origin;
                    willGenerateData.Add(willShowData);
                    
                    Mesh newMesh = MeshCutter.GenerateMesh(cuttedObjectData[j].Mesh, cuttedObjectData[j].Origin, planes[i], false, out intersectionPoints);
                    cuttedObjectData[j].Mesh = newMesh;
                }
            }
            
            foreach (var generateData in willGenerateData)
            {
                GameObject newObject = MeshCutter.CreateMeshObject(generateData.Origin.gameObject,generateData.Mesh,generateData.Origin.gameObject.name + "_Positive_Part" );
                newObject.transform.SetParent(canRemoveRoot);
                newObject.AddComponent<MeshCollider>();
            }
            
            foreach (var oldObject in oldObjects)
            {
                Destroy(oldObject);
            }
            
            
        }
        
        public void EnterCamAimState()
        {
            _polaroidStateMachine.ChangeState(EPolaroidState.CamAiming);
        }
        
        #endregion

        #region -------- Private Methods -------------

        
        private void BuildBackgroundQuad()
        {
            MeshFilter bgQuad = Instantiate(bgQuadPrefab, Vector3.zero, Quaternion.identity);
            
            Transform bgCamTransform = bgCamera.transform;
            
            bgQuad.transform.position = bgCamTransform.forward * bgCamera.nearClipPlane +  bgCamTransform.position;
            
            // Calculate the rotation needed to look at the target vector
            Quaternion targetRotation = Quaternion.LookRotation(bgCamTransform.forward);
            bgQuad.transform.rotation = targetRotation;
            
            // Set the scale to match the camera's frustum
            float tanValue = Mathf.Tan(AngleExtension.ToRadians(bgCamera.fieldOfView / 2));
            bgQuad.transform.localScale = Vector3.one * tanValue * bgCamera.nearClipPlane * 2;

            Texture2D text2D = ViewFinderExtension.ConvertToStaticTexture2D(bgRenderTexture);
            int mainTexHash = Shader.PropertyToID("_BaseMap");
            bgQuad.GetComponent<MeshRenderer>().material.SetTexture(mainTexHash, text2D);
            
            bgQuad.transform.SetParent(cuttedContainer);
        }
        


        #region ---------- Hold Polaroid State ------------

        private void EnterHoldPolaroidState()
        {
            _currentState = PolaroidState.HoldPolaroid;
            
            polaroidCamera.gameObject.SetActive(false);
            bgCamera.gameObject.SetActive(false);
            polaroidObject.SetActive(true);
            polaroidView.SetActive(false);
            photo.SetActive(false);
            
            polaroidObject.transform.position = polaroidHolder.position;
            polaroidObject.transform.rotation = polaroidHolder.rotation;

            _stateTimer = 0;

        }
        
        
        private void RunningHoldPolaroidState()
        {
            if(_currentState != PolaroidState.HoldPolaroid) return;
            
            _stateTimer += Time.deltaTime;
            
            if (Input.GetMouseButtonDown(0) && _stateTimer > 0.5f)
            {
                EnterAimPolaroidState();
            }
        }

        #endregion

        #region ---------- Aim Polaroid State ------------

        private void EnterAimPolaroidState()
        {
            _currentState = PolaroidState.AimPolaroid;
            
            polaroidCamera.gameObject.SetActive(true);
            bgCamera.gameObject.SetActive(true);
            polaroidView.SetActive(true);
            // photo.SetActive(false);
            
            polaroidObject.transform.position = polaroidAimPoint.position;
            polaroidObject.transform.rotation = polaroidAimPoint.rotation;
        }

        private void RunningAimPolaroidState()
        {
            if(_currentState != PolaroidState.AimPolaroid) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                CopyAndPasteSpaceInPolaroidCamView();
                EnterHoldPhotoState();
            }

            if (Input.GetMouseButtonDown(1))
            {
                EnterHoldPolaroidState();
            }
        }

        #endregion


        #region ----------- Hold Photo State ------------

        private void EnterHoldPhotoState()
        {
            _currentState = PolaroidState.HoldPhoto;
            
            polaroidCamera.gameObject.SetActive(false);
            bgCamera.gameObject.SetActive(false);
            polaroidObject.SetActive(false);
            // polaroidView.SetActive(true);
            photo.SetActive(true);
            
            // polaroidObject.transform.position = polaroidAimPoint.position;
            // polaroidObject.transform.rotation = polaroidAimPoint.rotation;
            
            photo.transform.position = photoHolder.position;
            photo.transform.rotation = photoHolder.rotation;
        }
        
        private void RunningHoldPhotoState()
        {
            if(_currentState != PolaroidState.HoldPhoto) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                EnterAimPhotoState();
            }
        }

        #endregion

        #region ------------ Aim Photo State -------------

        private void EnterAimPhotoState()
        {
            _currentState = PolaroidState.AimPhoto;
            
            // polaroidCamera.gameObject.SetActive(false);
            // polaroidView.SetActive(true);
            photo.SetActive(true);
            
            // polaroidObject.transform.position = polaroidAimPoint.position;
            // polaroidObject.transform.rotation = polaroidAimPoint.rotation;
            
            photo.transform.position = photoAimPoint.position;
            photo.transform.rotation = photoAimPoint.rotation;
        }

        private void RunningAimPhotoState()
        {
            if(_currentState != PolaroidState.AimPhoto) return;

            if (Input.GetMouseButtonDown(0))
            {
                //TO-DO
                // Cut space
                // show cutted space
                CutAndRemoveSpace();
                ReplaceParentForCuttedOjects();
                EnterHoldPolaroidState();
            }

            if (Input.GetMouseButtonDown(1))
            {
                EnterHoldPhotoState();
            }
        }

        

        #endregion

        #endregion
    }
}
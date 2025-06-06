using UnityEngine;

namespace MH.GamePlay.States
{
    public class HoldPhotoState : BasePolaroidState
    {
        public HoldPhotoState(StateMachine<EPolaroidState> stateMachine, PolaroidManager manager) : base(stateMachine, manager)
        {
            
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            //polaroidManager.polaroidCamera.gameObject.SetActive(false);
            //polaroidManager.bgCamera.gameObject.SetActive(false);
            //polaroidManager.polaroidObject.SetActive(false);
            // polaroidView.SetActive(true);
            polaroidManager.photo.SetActive(true);
            
            // polaroidObject.transform.position = polaroidAimPoint.position;
            // polaroidObject.transform.rotation = polaroidAimPoint.rotation;
            
            polaroidManager.photo.transform.position = polaroidManager.photoHolder.position;
            polaroidManager.photo.transform.rotation = polaroidManager.photoHolder.rotation;
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetMouseButtonDown(0))
            {
                _stateMachine.ChangeState(EPolaroidState.PhotoAiming);
            }
        }

        public override void OnExit()
        {
            
        }
    }
}
using UnityEngine;

namespace MH.GamePlay.States
{
    public class AimPhotoState : BasePolaroidState
    {
        GameObject photo => polaroidManager.photo;
        Transform photoAimPoint => polaroidManager.photoAimPoint;
        
        public AimPhotoState(StateMachine<EPolaroidState> stateMachine, PolaroidManager manager) : base(stateMachine, manager)
        {
            
        }

        public override void OnEnter()
        {
            photo.transform.position = photoAimPoint.position;
            photo.transform.rotation = photoAimPoint.rotation;
        }

        public override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                polaroidManager.CutAndRemoveSpace();
                polaroidManager.ReplaceParentForCuttedOjects();
                
                _stateMachine.ChangeState(EPolaroidState.None);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _stateMachine.ChangeState(EPolaroidState.HoldPhoto);
            }
        }

        public override void OnExit()
        {
            photo.SetActive(false);
        }
    }
}
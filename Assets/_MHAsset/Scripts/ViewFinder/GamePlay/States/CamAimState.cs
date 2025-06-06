using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.GamePlay.States
{
    public class CamAimState : BasePolaroidState
    {
        public CamAimState(StateMachine<EPolaroidState> stateMachine, PolaroidManager manager) : base(stateMachine, manager)
        {
            
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            // Logic for entering the CamAim state
            // For example, enabling camera controls or UI elements related to aiming
            polaroidManager.camAim_Cinema.SetActive(true);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            // Logic for updating the CamAim state
            // This could include checking for input to take a photo or exit the aim mode

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _stateMachine.ChangeState(EPolaroidState.None);
            }

            if (Input.GetMouseButtonDown(0))
            {
                TakePhotoProcess();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
            polaroidManager.camAim_Cinema.SetActive(false);
        }


        async void TakePhotoProcess()
        {
            polaroidManager.CopyAndPasteSpaceInPolaroidCamView();

            await UniTask.Delay(100);
            _stateMachine.ChangeState(EPolaroidState.HoldPhoto);
        }
    }
}
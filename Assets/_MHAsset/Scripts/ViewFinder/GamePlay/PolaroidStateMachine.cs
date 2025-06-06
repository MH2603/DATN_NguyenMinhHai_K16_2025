using MH.GamePlay.States;

namespace MH.GamePlay
{
    public enum EPolaroidState
    {
        None,
        CamAiming,
        PhotoTaking,
        HoldPhoto,
        PhotoAiming
    }
    
    public class PolaroidStateMachine : StateMachine<EPolaroidState>
    {
        public PolaroidStateMachine(PolaroidManager manager)
        {
            RegisterState(EPolaroidState.None, new NormalState(this, manager));
            RegisterState(EPolaroidState.CamAiming, new CamAimState(this, manager));
            RegisterState(EPolaroidState.PhotoTaking, new PhotoTakeState(this, manager));
            RegisterState(EPolaroidState.HoldPhoto, new HoldPhotoState(this, manager));
            RegisterState(EPolaroidState.PhotoAiming, new AimPhotoState(this, manager));
            
            ChangeState(EPolaroidState.None);
        }
    }
}
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
    
    public class PalaroidStateMachine : StateMachine<EPolaroidState>
    {
        
    }
}
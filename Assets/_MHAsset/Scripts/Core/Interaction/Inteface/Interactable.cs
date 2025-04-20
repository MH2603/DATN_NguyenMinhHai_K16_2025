namespace MH.Interaction
{
    public interface IInteractable
    {
        void EnterTracking();
        void ExitTracking();
        void Interact(IInteractor interactor);
        bool CanInteract(IInteractor interactor);
    }
}
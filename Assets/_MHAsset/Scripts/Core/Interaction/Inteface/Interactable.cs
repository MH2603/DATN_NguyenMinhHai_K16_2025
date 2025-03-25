namespace MH.Interaction
{
    public interface IInteractable
    {
        void OnTrackingEnter();
        void OnTrackingExit();
        void Interact(IInteractor interactor);
        bool CanInteract(IInteractor interactor);
    }
}
namespace MH.Interaction
{
    public interface IInteractable
    {
        void OnInteractableEnter();
        void OnInteractableExit();
        void Interact(IInteractor interactor);
        bool CanInteract(IInteractor interactor);
    }
}
namespace MH.Interaction
{
    public interface IInteractor
    {
        IInteractable InteractableTarget { get;  }
        void PerformInteraction(IInteractable interactable);
    }
}
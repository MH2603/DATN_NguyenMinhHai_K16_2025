using UnityEngine;

namespace MH.Interaction
{
    public abstract class IntertactorBase : MonoBehaviour, IInteractor
    {
        public IInteractable InteractableTarget { get; }
        public virtual void PerformInteraction(IInteractable interactable)
        {
            if (InteractableTarget != null)
            {
                InteractableTarget.Interact(this);
            }
        }
        
    }
    
    
}
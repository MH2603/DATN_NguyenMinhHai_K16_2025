using MH.EnitySystem;
using MH.Interaction;
using UnityEngine.Events;


namespace MH.Interaction
{
    public class InteractableComponent : EntityComponent, IInteractable
    {
        public UnityEvent OnEnterTracking;
        public UnityEvent OnExitTracking;
        public UnityEvent<IInteractor> OnInteraction;

        protected bool _isInteractable = true;       // Controls whether this can be interacted with


        public void SetLockInteract(bool on)
        {
            _isInteractable = !on;
        }

        // Called when the object becomes a potential target (e.g., raycast detects it)
        public virtual void EnterTracking()
        {
            OnEnterTracking?.Invoke();
        }

        // Called when the object is no longer a potential target
        public virtual void ExitTracking()
        {
            OnExitTracking?.Invoke();
        }

        // Called when the interactor performs an interaction (e.g., pickup or drop)
        public virtual void Interact(IInteractor interactor)
        {
            if (!_isInteractable) return;

            OnInteraction?.Invoke(interactor);
        }

        // Determines if the object can be interacted with
        public virtual bool CanInteract(IInteractor interactor)
        {
            // Example condition: Can interact if not already being held by another interactor
            return _isInteractable;
        }
    }
}

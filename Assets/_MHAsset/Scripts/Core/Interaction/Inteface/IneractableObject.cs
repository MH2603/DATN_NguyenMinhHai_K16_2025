using UnityEngine.Events;

namespace MH.Interaction
{
    using UnityEngine;
        
    public class InteractableObject : MonoBehaviour, IInteractable
    {

        public UnityEvent OnEnterTracking;
        public UnityEvent OnExitTracking;
        public UnityEvent OnInteraction;
        
        private bool isInteractable = true;       // Controls whether this can be interacted with
        private bool isInteracting = false;       // Tracks if the object is currently being interacted with

        
        // Called when the object becomes a potential target (e.g., raycast detects it)
        public void OnTrackingEnter()
        {
            OnEnterTracking?.Invoke();
        }

        // Called when the object is no longer a potential target
        public void OnTrackingExit()
        {
            OnExitTracking?.Invoke();
        }

        // Called when the interactor performs an interaction (e.g., pickup or drop)
        public void Interact(IInteractor interactor)
        {
            if (!isInteractable) return;

            // Toggle interaction state
            isInteracting = !isInteracting;

            OnInteraction?.Invoke();
        }

        // Determines if the object can be interacted with
        public bool CanInteract(IInteractor interactor)
        {
            // Example condition: Can interact if not already being held by another interactor
            return isInteractable && (!isInteracting || interactor.InteractableTarget == this);
        }
    }
}
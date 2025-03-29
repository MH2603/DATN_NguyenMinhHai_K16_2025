using UnityEngine;

namespace MH.Interaction
{
    public abstract class IntertactorBase : MonoBehaviour, IInteractor
    {
        #region ------------ Fields -------------

        protected IInteractable detectedInteractable; 

        #endregion


        #region -------------- Unity Methods ----------------- 

        #endregion

        #region ---------------- Public Methods ----------------

        public virtual void PerformInteraction()
        {
            if (detectedInteractable == null || !detectedInteractable.CanInteract(this)) return;
        
            detectedInteractable.Interact(this);
        }

        #endregion

        protected abstract void DetectInteractable();
       

    }
    
    
}
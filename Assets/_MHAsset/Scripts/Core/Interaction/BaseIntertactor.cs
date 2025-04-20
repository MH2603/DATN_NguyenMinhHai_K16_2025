using UnityEngine;

namespace MH.Interaction
{
    public abstract class BaseIntertactor : MonoBehaviour, IInteractor
    {
        #region ------------ Fields -------------

        protected IInteractable _trackingInteractable; 

        #endregion


        #region -------------- Unity Methods ----------------- 

        #endregion

        #region ---------------- Public Methods ----------------

        public virtual void PerformInteraction()
        {
            if (_trackingInteractable == null || !_trackingInteractable.CanInteract(this)) return;
        
            _trackingInteractable.Interact(this);
        }

        #endregion
    }
    
    
}
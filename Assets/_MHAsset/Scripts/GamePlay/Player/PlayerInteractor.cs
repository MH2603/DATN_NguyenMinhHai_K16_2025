using MH.EnitySystem;
using MH.Interaction;
using MH.UISystem;
using UnityEngine;

namespace MH.Player
{
    public class PlayerInteractor : EntityComponent, IInteractor
    {
        #region ------------ Fields -----------

        [SerializeField] private float _detectDst = 5f;

        private IInteractable _trackingInteractable;
        private Transform _trans;

        #endregion

        #region ----------------- Unity Methods ------------



        #endregion


        #region ----------- Public Methods ------------

        #region ------ Entity Methods --------

        public override void Initialized(BaseEnitity baseEnitity)
        {
            base.Initialized(baseEnitity);

            _trackingInteractable = null;
            _trans = this.transform;
        }

        public override void ManualUpdate()
        {
            base.ManualUpdate();

            DetectingInteractable();

            // todo: get callback from InputManager
            if (Input.GetKeyDown(KeyCode.E))
            {
                PerformInteraction();
            }
        }

        #endregion

        public void PerformInteraction()
        {
            if(_trackingInteractable != null && _trackingInteractable.CanInteract(this))
            {
                _trackingInteractable.Interact(this);

                _trackingInteractable.ExitTracking();
                _trackingInteractable = null;
            }
        } 


        #endregion


        private void DetectingInteractable()
        {
            if( Physics.Raycast(_trans.position, _trans.forward, out RaycastHit hit, _detectDst) &&
                hit.transform &&
                hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (_trackingInteractable != null && interactable != _trackingInteractable)
                {
                    _trackingInteractable.ExitTracking();
                }

                interactable.EnterTracking();
                _trackingInteractable = interactable;

                InteractHUDViewModel vm = new InteractHUDViewModel
                {
                    InteractableTrans = hit.transform
                };

                HUDLayer.Main.ShowAsync<InteractHUD>(vm);

#if UNITY_EDITOR
                DebugDrawer.DrawRay(_trans.position, _trans.forward * _detectDst, Color.red, 0.02f); 
#endif
            }
            else
            {
                if (_trackingInteractable != null)
                {
                    _trackingInteractable.ExitTracking();
                }

                _trackingInteractable = null;
                HUDLayer.Main.HideAsync<InteractHUD>();

#if UNITY_EDITOR
                DebugDrawer.DrawRay(_trans.position, _trans.forward * _detectDst, Color.green, 0.02f); 
#endif
            }
        }


    }
}


using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    public class InteractHUDViewModel : IViewModel
    {
        public Transform InteractableTrans;
    }

    public class InteractHUD : UIView
    {

        [SerializeField] private RectTransform _text;

        private Transform _lastInteractableTrans;


        private void Update()
        {
            SetWidgetPos();
        }

        public override void LoadViewModel(IViewModel viewModel)
        {
            base.LoadViewModel(viewModel);

            var vm = viewModel as InteractHUDViewModel;

            if (vm != null) 
            {
                _lastInteractableTrans = vm.InteractableTrans;
            }
            else
            {
                _lastInteractableTrans = null;
            }
        }

        private void SetWidgetPos()
        {
            if(_lastInteractableTrans == null)
            {
                return;
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(_lastInteractableTrans.position);
            _text.position = screenPos;
        }
    }
}

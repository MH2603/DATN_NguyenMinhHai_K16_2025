using UnityEngine;
using UnityEngine.Events;

namespace MH.DialogSystem
{
    public class DialogInvoker : MonoBehaviour
    {
        [SerializeField] private DialogSequenceSO config;
        [SerializeField] private UnityEvent EnterDialog;
        [SerializeField] private UnityEvent ExitDialog;

        private IDialogSystem _dialogSystem => ServiceLocator.Get<IDialogSystem>();
        
        private void Start()
        {
            
        }
        
        public void StartDialog()
        {
            _dialogSystem.StartSequence(config);
            _dialogSystem.OnSequenceComplete.AddListener(OnDialogComplete);
            EnterDialog?.Invoke();
        }

        void OnDialogComplete()
        {
            ExitDialog?.Invoke();
            _dialogSystem.OnSequenceComplete.AddListener(OnDialogComplete);
        }
    }
}
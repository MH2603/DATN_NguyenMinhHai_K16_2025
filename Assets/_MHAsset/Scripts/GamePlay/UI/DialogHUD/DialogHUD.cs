using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MH.UISystem
{
    public class DialogueViewModel : IViewModel
    {
        public string DialogText;
        public string SpeakerName;
        
        public Action<int> OnCharacterVisibleUpdated;
        public UnityEvent<int> OnCharacterVisibleUpdatedEvent;
    }
    
    public class DialogHUD : UIView
    {
        [SerializeField] private TextMeshProUGUI _speakerNameText;
        [SerializeField] private TextMeshProUGUI _dialogText;
        
        
        public override void LoadViewModel(IViewModel viewModel)
        {
            base.LoadViewModel(viewModel);
            
            var vm = viewModel as DialogueViewModel;
            
            _dialogText.text = vm.DialogText;
            _speakerNameText.text = vm.SpeakerName;
            _dialogText.maxVisibleCharacters = 0; // Reset to show all characters
            
            Debug.Log("Registering OnCharacterVisibleUpdated");
            //vm.OnCharacterVisibleUpdated += UpdateCharacterVisible;
            vm.OnCharacterVisibleUpdatedEvent.AddListener(UpdateCharacterVisible);
        }
        
        public void UpdateCharacterVisible(int visibleCount)
        {
            //Debug.Log("UpdateCharacterVisible: " + visibleCount);
            _dialogText.maxVisibleCharacters = visibleCount;
        }
    }
}
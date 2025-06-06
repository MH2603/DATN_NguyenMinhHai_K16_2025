using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MH.UISystem;
using UnityEngine;
using UnityEngine.Events;

namespace MH.DialogSystem
{
    public enum EDialogState
    {
        None,
        SentenceRunning,
        WaitNextSentence
    }
    
    public interface IDialogSystem
    {
        UnityEvent OnSequenceComplete { get; set; }

        // Define methods for dialog system
        void StartSequence(DialogSequenceSO sequence);

        void ForceCompleteSequence();

    }
    
    public class DialogSystem : MonoBehaviour, IDialogSystem
    {
        #region ----------- FIELDS ----------------

        public UnityEvent OnSequenceComplete { get; set; }
        
        private EDialogState _currentState = EDialogState.None;
        //private Action<int> _onCharacterVisibleUpdated;
        private UnityEvent<int> _onCharacterVisibleUpdatedEvent = new();

        private DialogSequenceSO _currentSequence;
        private int _sentenceIndex = 0;
        private CancellationTokenSource cst = new CancellationTokenSource();

        #endregion

        #region ----------- UNITY METHODS -------------

        private void Update()
        {
            if ( Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) )
            {
                switch (_currentState)
                {
                    case EDialogState.SentenceRunning:
                        TrySkipRunningSentence();
                        break;
                    case EDialogState.WaitNextSentence:
                        TryNextSentence();
                        break;
                }
                
                
            }
            
        }

        #endregion
        
        public void StartSequence(DialogSequenceSO sequence)
        {
            _currentState = EDialogState.SentenceRunning;
            _currentSequence = sequence;
            _sentenceIndex = 0;
            
            OnSequenceComplete = new UnityEvent();
            
            RunSentenceAsync();
        }
        
        public void ForceCompleteSequence()
        {
            // Force complete the current sequence
            if (_currentState == EDialogState.SentenceRunning)
            {
                cst.Cancel(); // Cancel any running sentence
                _currentState = EDialogState.WaitNextSentence; // Set to wait state
            }
            
            // Hide dialog HUD immediately
            HUDLayer.Main.HideAsync<DialogHUD>();
            _currentState = EDialogState.None;
            
            OnSequenceComplete?.RemoveAllListeners();
            //OnSequenceComplete?.Invoke();
        }


        async void RunSentenceAsync()
        {
            var sequenceConfig = _currentSequence;
            var sentenceData = _currentSequence.Sentences[_sentenceIndex];
            
            string sentenceText = sentenceData.Content;
            int characterMax = sentenceText.Length;

            ShowDialogHUD(sequenceConfig.Speaker, sentenceText);
            
            var cts = new System.Threading.CancellationTokenSource();

            for (int i = 0; i < characterMax; i++)
            {
                if (_currentState == EDialogState.WaitNextSentence)
                {
                    //_onCharacterVisibleUpdated?.Invoke(characterMax);
                    _onCharacterVisibleUpdatedEvent?.Invoke(characterMax);
                    break;
                }
                    
                // Wait for the duration of each character with cancellation support
                await UniTask.Delay(sequenceConfig.CharacterDur, cancellationToken: cts.Token);

                // Update the visible characters in the dialog HUD
                //_onCharacterVisibleUpdated.Invoke(i + 1);
                _onCharacterVisibleUpdatedEvent.Invoke(i + 1);
            }

            _sentenceIndex++;
            _currentState = EDialogState.WaitNextSentence;
        }

        void ShowDialogHUD(string speaker, string dialogText)
        {
            // Initialize the action to handle character visibility updates
            //_onCharacterVisibleUpdated = visibleCount =>
            {
                //Debug.Log($"Visible characters updated: {visibleCount}");
            };
            
            var dialogViewModel = new DialogueViewModel
            {
                SpeakerName = speaker,
                DialogText = dialogText,
                //OnCharacterVisibleUpdated = _onCharacterVisibleUpdated,
                OnCharacterVisibleUpdatedEvent = _onCharacterVisibleUpdatedEvent
            };

            HUDLayer.Main.ShowAsync<DialogHUD>(dialogViewModel);
        }

        void TrySkipRunningSentence()
        {
            if (_currentState == EDialogState.SentenceRunning
                 )
            {
                // Skip the current sentence
                _currentState = EDialogState.WaitNextSentence;
            }
        }
        
        void TryNextSentence()
        {
            if (_currentState != EDialogState.WaitNextSentence) return;
           
            if (_sentenceIndex < _currentSequence.Sentences.Length)
            {
                _currentState = EDialogState.SentenceRunning;
                RunSentenceAsync();
            }
            else
            {
                // End of sequence, hide dialog HUD
                HUDLayer.Main.HideAsync<DialogHUD>();
                _currentState = EDialogState.None;
                
                OnSequenceComplete?.Invoke();
            }
        }
       
    }
}
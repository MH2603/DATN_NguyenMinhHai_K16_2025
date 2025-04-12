using MH.EventBus;
using UnityEngine;

namespace MH.GameState
{
    public enum EGameState
    {
        Loading,
        MainMenu,
        Playing,
        Pause,
        Exit
    }

    public interface IGameStateSystem
    {
        EGameState CurrentState { get; }
        void ChangeState(EGameState newState);

    }

    public class GameStateSystem : StateMachine<EGameState>, IGameStateSystem
    {
        private IEventBus _eventBus => ServiceLocator.Get<IEventBus>();

        public EGameState CurrentState { get; private set; }

        public GameStateSystem() 
        {
            // Register all game states here
            RegisterState(EGameState.Loading, new LoadingState(this));
            RegisterState(EGameState.MainMenu, new MainMenuState(this));
            RegisterState(EGameState.Playing, new PlayingState(this));
            RegisterState(EGameState.Pause, new PauseState(this));
            RegisterState(EGameState.Exit, new ExitState(this));

            ChangeState(EGameState.Loading);
        }

        public override void ChangeState(EGameState key)
        {
            if (_states.TryGetValue(key, out var newState))
            {
                // Exit current state if it exists
                _currentState?.OnExit();
                _eventBus.Send(new GameStateChangeEvent(_currentKey, key));

                // Transition to new state
                _currentState = newState;
                _currentKey = key;

                // Enter new state
                _currentState.OnEnter();
            }
            else
            {
                // Developer feedback if state wasn't registered
                Debug.LogWarning($"State {key} not registered.");
            }
        }
    }
}

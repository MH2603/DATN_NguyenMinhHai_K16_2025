using MH.EventBus;

namespace MH.GameState
{
    public class GameStateChangeEvent : IEventContext
    {
        public EGameState ExitState;
        public EGameState EnterState;

        public GameStateChangeEvent(EGameState enterState, EGameState exitState)
        {
            ExitState = enterState;
            EnterState = exitState;
        }
    }
}

using System;

namespace MH.GameState
{
    public class LoadingState : BaseGameState
    {
        public LoadingState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
            
        }

        public override void OnEnter()
        {
            _stateMachine.ChangeState(EGameState.MainMenu);
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        
    }
}

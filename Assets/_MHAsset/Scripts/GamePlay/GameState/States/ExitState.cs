using System;

namespace MH.GameState
{
    public class ExitState : BaseGameState
    {
        public ExitState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}

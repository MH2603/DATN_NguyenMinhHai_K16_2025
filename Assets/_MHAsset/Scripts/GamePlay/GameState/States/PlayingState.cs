using System;
using UnityEngine;

namespace MH.GameState
{
    public class PlayingState : BaseGameState
    {
        public PlayingState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            Debug.Log(" On Playing State Enter");
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) &&
                _stateMachine.CurrentKey == EGameState.Playing)
            {
                _stateMachine.ChangeState(EGameState.Pause);
            }
        }
    }
}

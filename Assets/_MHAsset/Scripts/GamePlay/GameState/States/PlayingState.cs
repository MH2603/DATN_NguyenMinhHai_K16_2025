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
            
        }
    }
}

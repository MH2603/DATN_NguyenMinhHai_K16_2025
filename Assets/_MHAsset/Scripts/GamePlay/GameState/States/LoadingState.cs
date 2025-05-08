using System;
using Cysharp.Threading.Tasks;
using MH.UISystem;
using UnityEngine;

namespace MH.GameState
{
    public class LoadingState : BaseGameState
    {
        public LoadingState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
            
        }

        public override void OnEnter()
        {
            TranderMainMenuStateAsync();
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        private async void TranderMainMenuStateAsync()
        {
            WindowLayer.Main.ShowAsync<LoadingWindow>();
            
            await UniTask.Delay(2000);

             await WindowLayer.Main.BackAsync();
            _stateMachine.ChangeState(EGameState.MainMenu);
            Debug.Log($"from loading to main menu: {_stateMachine.CurrentKey}");
        }
    }
}

using System;
using Cysharp.Threading.Tasks;
using MH.GraphicQuality;
using MH.SaveSystem;
using MH.UISystem;
using UnityEngine;

namespace MH.GameState
{
    public class LoadingState : BaseGameState
    {
        private ISaveSystem saveSystem => ServiceLocator.Get<ISaveSystem>();
        private IGraphicQualityManager graphicQualityManager => ServiceLocator.Get<IGraphicQualityManager>();
        
        public LoadingState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
            
        }

        public override void OnEnter()
        {
            TranferMainMenuStateAsync();
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        private async void TranferMainMenuStateAsync()
        {
            WindowLayer.Main.ShowAsync<LoadingWindow>();
            
            await UniTask.Delay(4000);

             await WindowLayer.Main.BackAsync();
            _stateMachine.ChangeState(EGameState.MainMenu);
            Debug.Log($"from loading to main menu: {_stateMachine.CurrentKey}");
        }

        private void InitSetting()
        {
            var save = saveSystem.GameSave;
            
        }
    }
}

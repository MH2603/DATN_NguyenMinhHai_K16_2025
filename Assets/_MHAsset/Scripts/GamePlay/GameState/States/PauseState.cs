

using MH.Player;
using MH.SceneLoader;
using MH.UISystem;
using UnityEngine;

namespace MH.GameState
{
    public class PauseState : BaseGameState
    {

        //private IPlayerManager playerManager => ServiceLocator.Get<IPlayerManager>();
        private ISceneLoader sceneLoader => ServiceLocator.Get<ISceneLoader>();
        
        public PauseState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;

            //playerManager.SetLockControl(true);
            
            ShowPauseWindow();

        }

        public override void OnExit()
        {
            Time.timeScale = 1;
            
        }

        public override void OnUpdate()
        {
            
        }

        private void ShowPauseWindow()
        {
            var pauseVM = new PauseViewModel
            {
                OnResumeGame =  OnResume,
                OnMainMenu = OnMainMenu,
                OnOptions = OnOptions
            };
            
            WindowLayer.Main.ShowAsync<PauseWindow>(pauseVM);
        }

        void OnResume()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            WindowLayer.Main.BackAsync();
            
            //playerManager.SetLockControl(false);
            
            _stateMachine.ChangeState(EGameState.Playing);
        }

        async void OnMainMenu()
        {
            Time.timeScale = 1;
            
            await TransitionLayer.Main.ShowAsync<FadeTransition>();
            await WindowLayer.Main.BackAsync();
            
            Debug.Log("OnMainMenu");
            
            sceneLoader.UnloadSceneAsync(sceneLoader.SceneHistory[sceneLoader.SceneHistory.Count - 1]);
            
            _stateMachine.ChangeState(EGameState.MainMenu);
        }

        void OnOptions()
        {
            WindowLayer.Main.ShowAsync<SettingWindow>();
        }
    }
}

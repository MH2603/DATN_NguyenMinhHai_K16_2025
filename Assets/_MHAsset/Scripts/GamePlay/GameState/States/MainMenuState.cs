using MH.ChapterSystem;
using MH.DialogSystem;
using MH.SaveSystem;
using MH.UISystem;
using UnityEngine;

namespace MH.GameState
{
    public class MainMenuState : BaseGameState
    {
        private ISaveSystem _saveSystem => ServiceLocator.Get<ISaveSystem>();
        private IChapterManager _chapterManager => ServiceLocator.Get<IChapterManager>();
        
        public MainMenuState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            //Debug.Log(" On Main Menu State Enter");

            ServiceLocator.Get<IDialogSystem>().ForceCompleteSequence();
            OpenMainMenuWindowAsync();
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        public async void OpenMainMenuWindowAsync()
        {
            // set up VM
            var save = ServiceLocator.Get<ISaveSystem>().GameSave;
            bool isNewGame = save.Chapter == 0 && save.CheckPoint == 0;

            var mainMenuVM = new MainMenuViewModel
            {
                IsShowContinueBtn = !isNewGame,
                OnNewGameBtnClicked = OnNewGame,
                OnContinueBtnClicked = OnContinue,
                OnOptionBtnClicked = OnOptions,
                OnCreditBtnClicked = OnCredit,
                OnExitGameBtnClicked = OnExitBtnPress
            };

            await WindowLayer.Main.ShowAsync<MainMenuWindow>(mainMenuVM);

            await TransitionLayer.Main.HideAsync<FadeTransition>();
        }
        
        private async void OnNewGame()
        {
            // _saveSystem.Clean();
            // _chapterManager.SwithChapter(0);
            // ServiceLocator.Get<IGameStateSystem>().ChangeState(EGameState.Playing);
            //
            // await  WindowLayer.Main.BackAsync();

            WindowLayer.Main.ShowAsync<MapSelectionWindow>();
        }

        async void OnContinue()
        {
            var save = _saveSystem.GameSave;  
            _chapterManager.SwithChapter(save.Chapter, save.CheckPoint);

            WindowLayer.Main.BackAsync();
        }
        
        void OnOptions()
        {
            //Debug.Log("On Options");
            
            WindowLayer.Main.ShowAsync<SettingWindow>();
        }
        
        void OnCredit()
        {
            //Debug.Log("On Credit");
            WindowLayer.Main.ShowAsync<CreditWindow>();
        }

        void OnExitBtnPress()
        {
            Application.Quit();
        }
    }
}

using MH.SaveSystem;
using MH.UISystem;

namespace MH.GameState
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(StateMachine<EGameState> stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            var save = ServiceLocator.Get<ISaveSystem>().GameSave;
            bool isNewGame = save.Chapter == 0 && save.CheckPoint == 0;

            var mainMenuVM = new MainMenuViewModel
            {
                IsShowContinueBtn = !isNewGame
            };

            WindowLayer.Main.ShowAsync<MainMenuWindow>(mainMenuVM);
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}

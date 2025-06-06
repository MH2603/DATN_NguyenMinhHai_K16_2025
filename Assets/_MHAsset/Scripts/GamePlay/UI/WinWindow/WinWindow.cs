using MH.GameState;
using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    public class WinWindow : UIView
    {
        [SerializeField] private Button mainmenuButton;
        
        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            mainmenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
        
        private void OnMainMenuButtonClicked()
        {
            // Logic to navigate to the main menu
            //WindowLayer.Main.BackAsync();
            // You can also add any additional logic here, like saving game state or showing a confirmation dialog.
            
            ServiceLocator.Get<IGameStateSystem>().ChangeState(EGameState.MainMenu);
        }
    }
}
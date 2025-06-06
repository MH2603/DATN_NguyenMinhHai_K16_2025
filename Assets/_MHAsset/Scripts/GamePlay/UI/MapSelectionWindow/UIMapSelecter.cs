using System;
using Cysharp.Threading.Tasks;
using MH.GameState;
using MH.SceneLoader;
using UnityEngine;
using UnityEngine.UI;

namespace MH.UISystem
{
    public class UIMapSelecter : UIWidget
    {
        [SerializeField] private CustomButton selectButton;

        private void Start()
        {
            selectButton.onClick.AddListener(SelectMap);
        }

        async void SelectMap()
        {
            TransitionLayer.Main.ShowAsync<FadeTransition>();
            await UniTask.Delay(1000);
            
            ServiceLocator.Get<ISceneLoader>().LoadSceneAsync(ID, LoadSceneComplete);
        }

        async void LoadSceneComplete()
        {
            WindowLayer.Main.BackAsync(true);
            await UniTask.Delay(500);
            TransitionLayer.Main.HideAsync<FadeTransition>();
            ServiceLocator.Get<IGameStateSystem>().ChangeState(EGameState.Playing);
        }
    }
}
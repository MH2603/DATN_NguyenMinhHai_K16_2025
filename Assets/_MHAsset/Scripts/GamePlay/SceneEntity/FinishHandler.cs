using Cysharp.Threading.Tasks;
using MH.EnitySystem;
using MH.SceneLoader;
using MH.UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace MH
{
    public class FinishHandler : EntityComponent
    {
        [SerializeField] private string nextSceneName;
        [SerializeField] private string unloadScene;
        private ISceneLoader SceneLoader => ServiceLocator.Get<ISceneLoader>();
        
        public void OnFinish()
        {
            AsyncFinishProcess();
        }

        async void AsyncFinishProcess()
        {
            Debug.Log(" FinishHandler.OnFinish");
            TransitionLayer.Main.ShowAsync<FadeTransition>();
            await UniTask.Delay(500);
            
            SceneLoader.UnloadSceneAsync(unloadScene);
            SceneLoader.LoadSceneAsync(nextSceneName);
            
            await UniTask.Delay(1000);
            
            TransitionLayer.Main.HideAsync<FadeTransition>();
        }
    }
}
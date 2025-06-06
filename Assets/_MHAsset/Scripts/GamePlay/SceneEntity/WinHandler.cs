using Cysharp.Threading.Tasks;
using MH.SceneLoader;
using MH.UISystem;
using UnityEngine;

namespace MH
{
    public class WinHandler : MonoBehaviour
    {
        [SerializeField] private string unloadScene = "Chapter_04";
        
        public async void ShowWin()
        {
            Cursor.lockState = CursorLockMode.Confined;
            WindowLayer.Main.ShowAsync<WinWindow>();
            ServiceLocator.Get<ISceneLoader>().UnloadSceneAsync(unloadScene);
        }
            
    }
}
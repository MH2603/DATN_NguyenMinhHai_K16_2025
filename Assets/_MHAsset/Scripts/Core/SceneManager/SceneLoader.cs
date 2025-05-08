using System;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace MH.SceneLoader
{

    public interface ISceneLoader
    {
        void LoadScene(string sceneName);
        void UnloadScene(string sceneName);
        
        void LoadSceneAsync(string sceneName);
        void UnloadSceneAsync(string sceneName);
        
        void LoadScene(string sceneName, System.Action onLoaded);
        void UnloadScene(string sceneName, System.Action onUnloaded);
        
        void LoadSceneAsync(string sceneName, System.Action onLoaded);
        void UnloadSceneAsync(string sceneName, System.Action onUnloaded);
    }
    
    public class SceneLoader : ISceneLoader
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public void LoadSceneAsync(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public void UnloadSceneAsync(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        public void LoadScene(string sceneName, Action onLoaded)
        {
            SceneManager.LoadScene(sceneName);
            onLoaded?.Invoke();
        }

        public void UnloadScene(string sceneName, Action onUnloaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            onUnloaded?.Invoke();
        }

        public void LoadSceneAsync(string sceneName, Action onLoaded)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.completed += _ => onLoaded?.Invoke();
        }

        public void UnloadSceneAsync(string sceneName, Action onUnloaded)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            operation.completed += _ => onUnloaded?.Invoke();
        }
    }
}
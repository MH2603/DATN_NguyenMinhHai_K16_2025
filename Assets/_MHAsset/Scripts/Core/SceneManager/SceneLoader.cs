using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace MH.SceneLoader
{

    public interface ISceneLoader
    {
        List<string> SceneHistory { get; }
        void LoadSceneAsync(string sceneName, System.Action onLoaded = null, LoadSceneMode mode = LoadSceneMode.Additive);
        void UnloadSceneAsync(string sceneName, System.Action onUnloaded = null, LoadSceneMode mode = LoadSceneMode.Additive);
    }
    
    public class SceneLoader : ISceneLoader
    {
        public List<string> SceneHistory { get => _sceneHistory; }
        
        private List<string> _sceneHistory = new();

        public void LoadSceneAsync(string sceneName, Action onLoaded, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            operation.completed += _ =>
            {
                onLoaded?.Invoke();
                _sceneHistory.Add(sceneName);
            };
        }

        public void UnloadSceneAsync(string sceneName, Action onUnloaded = null, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);
            operation.completed += _ => onUnloaded?.Invoke();
        }
        
    }
}
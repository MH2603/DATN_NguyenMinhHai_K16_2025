

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MH.SaveSystem;
using MH.Player;
using MH.SceneLoader;
using MH.UISystem;
using UnityEngine;

namespace MH.ChapterSystem
{
    public interface IChapterManager
    {
        void SwithChapter(int nextChapterIndex, int checkPointIndex=0);
       
    }

    public class ChapterManager : IChapterManager
    {
        #region ------------- Fields ------------------
        private Dictionary<int, ChapterConfig> _chapterConfigMap = new();

        private IPlayerManager playerManager => ServiceLocator.Get<IPlayerManager>();
        private ISaveSystem saveSystem => ServiceLocator.Get<ISaveSystem>();
        private ISceneLoader sceneLoader => ServiceLocator.Get<ISceneLoader>();

        #endregion

        #region -------------- Public Methods ------------------


        public ChapterManager(ChapterConfig[] chapterConfigs)
        {
            for (int i=0; i < chapterConfigs.Length; i++)
            {
                var chapter = chapterConfigs[i];
                _chapterConfigMap.Add(chapter.Id, chapter);
            }
          
        }


        public async void SwithChapter(int nextChapterIndex, int checkPointIndex=0)
        {
            playerManager.SetLockControl(true);

            await TransitionLayer.Main.ShowAsync<FadeTransition>();

            await UniTask.NextFrame();
            
            
            // var nextChapterPrefab = _chappterPrefabs[nextChapterIndex];
            // _currentChapter = GameObject.Instantiate( nextChapterPrefab, this.transform);

            var chapterConfig = _chapterConfigMap[nextChapterIndex];
            sceneLoader.LoadSceneAsync(chapterConfig.SceneName);
            
            await UniTask.NextFrame();
            
            // hard-code
            playerManager.Teleport(Vector3.up, Quaternion.identity);

            saveSystem.GameSave.Chapter = nextChapterIndex;
            saveSystem.Save();

            await UniTask.Delay(300);

            await TransitionLayer.Main.HideAsync<FadeTransition>();

            playerManager.SetLockControl(false);
        } 

        #endregion
    }
}

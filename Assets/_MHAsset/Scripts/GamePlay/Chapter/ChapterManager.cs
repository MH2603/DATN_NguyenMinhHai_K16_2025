

using Cysharp.Threading.Tasks;
using MH.SaveSystem;
using MH.Player;
using MH.UISystem;
using UnityEngine;

namespace MH.ChapterSystem
{
    public interface IChapterManager
    {
        Chapter CurrentChapter { get; }

        void SwithChapter(int nextChapterIndex, int checkPointIndex=0);
       
    }

    public class ChapterManager : MonoBehaviour, IChapterManager
    {
        #region ------------- Fields ------------------
        [SerializeField] private Chapter[] _chappterPrefabs;

        private Chapter _currentChapter;

        private IPlayerManager _playerManager => ServiceLocator.Get<IPlayerManager>();
        private ISaveSystem _saveSystem => ServiceLocator.Get<ISaveSystem>();
        public Chapter CurrentChapter => _currentChapter;

        #endregion

        #region -------------- Public Methods ------------------


        public ChapterManager(Chapter[] chapters)
        {
            _chappterPrefabs = chapters;
        }


        public async void SwithChapter(int nextChapterIndex, int checkPointIndex=0)
        {
            _playerManager.SetLockControl(true);

            await TransitionLayer.Main.ShowAsync<FadeTransition>();

            await UniTask.NextFrame();

            if (_currentChapter)
            {
                GameObject.Destroy(_currentChapter.gameObject);
            }

            var nextChapterPrefab = _chappterPrefabs[nextChapterIndex];
            _currentChapter = GameObject.Instantiate( nextChapterPrefab, this.transform);

            await UniTask.NextFrame();

            var telePoint = _currentChapter.GetCheckPoint(nextChapterIndex);
            _playerManager.Teleport(telePoint.position, telePoint.rotation);

            _saveSystem.GameSave.Chapter = nextChapterIndex;
            _saveSystem.Save();

            await UniTask.Delay(300);

            await TransitionLayer.Main.HideAsync<FadeTransition>();

            _playerManager.SetLockControl(false);
        } 

        #endregion
    }
}

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.ChapterSystem
{
    [CreateAssetMenu(fileName = "ChapterSystem", menuName = "MH_SO/GameSystem/Chapter System")]
    public class ChapterSystemInitializer : GameSystemInitializer
    {
        [SerializeField] private ChapterManager _managerPrefab;

        public override async UniTask Initialize()
        {
            var manager = GameObject.Instantiate(_managerPrefab);

            ServiceLocator.Register<IChapterManager>(manager);
        }
    }
}

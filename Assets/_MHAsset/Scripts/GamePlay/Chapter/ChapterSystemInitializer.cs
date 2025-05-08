using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.ChapterSystem
{
    [CreateAssetMenu(fileName = "ChapterSystem", menuName = "MH_SO/GameSystem/Chapter System")]
    public class ChapterSystemInitializer : GameSystemInitializer
    {
        [SerializeField] private ChapterConfig[] _chapterConfigs;
        
        public override async UniTask Initialize()
        {
            var manager = new ChapterManager(_chapterConfigs);

            ServiceLocator.Register<IChapterManager>(manager);
        }
    }
}

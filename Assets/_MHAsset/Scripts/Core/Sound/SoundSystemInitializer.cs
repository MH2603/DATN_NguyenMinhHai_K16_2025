

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.Sound
{
    [CreateAssetMenu(fileName = "SoundSystem", menuName = "MH_SO/GameSystem/SoundSystem")]
    public class SoundSystemInitializer : GameSystemInitializer
    {
        [SerializeField] private SoundManager soundManagerPrefab;

        public override async UniTask Initialize()
        {
            var soundManager = GameObject.Instantiate(soundManagerPrefab);

            ServiceLocator.Register<ISoundManager>(soundManager);

        }
    }
}

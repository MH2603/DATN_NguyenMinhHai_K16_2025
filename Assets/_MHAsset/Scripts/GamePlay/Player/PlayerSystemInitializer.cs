
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.Player
{
    [CreateAssetMenu(fileName = "PlayerSystem", menuName = "MH_SO/GameSystem/Player System")]
    public class PlayerSystemInitializer : GameSystemInitializer
    {
        [SerializeField] private PlayerEntity _playerEntity;

        public override async UniTask Initialize()
        {
            var playerManager = new PlayerManager(_playerEntity);
            await UniTask.NextFrame();

            ServiceLocator.Register<IPlayerManager>(playerManager);
        }
    }
}


using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.GameState
{
    [CreateAssetMenu(fileName = "GameState", menuName = "MH_SO/GameSystem/GameState")]
    public class GameStateInitializer : GameSystemInitializer
    {
        public override async UniTask Initialize()
        {
            var gameStateSystem  = new GameStateSystem();

            ServiceLocator.Register<IGameStateSystem>(gameStateSystem);
        }
    }
}

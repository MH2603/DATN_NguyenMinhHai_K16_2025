using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.SceneLoader
{
    [CreateAssetMenu(fileName = "SceneSystem", menuName = "MH_SO/GameSystem/SceneSystem")]
    public class SceneManagerInitializer : GameSystemInitializer
    {
        public override async UniTask Initialize()
        {
            var sceneLoader = new SceneLoader();
            
            ServiceLocator.Register<ISceneLoader>(sceneLoader);
        }
    }

}
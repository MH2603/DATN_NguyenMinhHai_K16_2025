using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.SaveSystem
{
    [CreateAssetMenu(fileName = "SaveSystem", menuName = "MH_SO/GameSystem/Save System")]
    public class SaveSystemInitializer : GameSystemInitializer
    {

        public override async UniTask Initialize()
        {
            base.Initialize();

            var saveSystem = new SaveSystem();
            saveSystem.Load();

            ServiceLocator.Register<ISaveSystem>(saveSystem);
        }
    }
}

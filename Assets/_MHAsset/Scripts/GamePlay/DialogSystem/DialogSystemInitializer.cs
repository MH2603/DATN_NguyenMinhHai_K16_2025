using Cysharp.Threading.Tasks;
using MH.EventBus;
using MH.GameState;
using UnityEngine;

namespace MH.DialogSystem
{
    [CreateAssetMenu(fileName = "DialogSystem", menuName = "MH_SO/GameSystem/DialogSystem")]
    public class DialogSystemInitializer : GameSystemInitializer
    {
        public override async UniTask Initialize()
        {
            var system = new GameObject().AddComponent<DialogSystem>();
            system.name = "DialogSystem";
            ServiceLocator.Register<IDialogSystem>(system);
           
        }
    }
}
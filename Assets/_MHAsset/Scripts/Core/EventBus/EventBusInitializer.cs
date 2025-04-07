
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH.EventBus
{
    [CreateAssetMenu(fileName = "EventBus", menuName = "MH_SO/GameSystem/EventBus")]
    public class EventBusInitializer : GameSystemInitializer
    {
        public override async UniTask Initialize()
        {
            var eventBus = new EventBus();

            ServiceLocator.Register<IEventBus>(eventBus);
        }
    }
}

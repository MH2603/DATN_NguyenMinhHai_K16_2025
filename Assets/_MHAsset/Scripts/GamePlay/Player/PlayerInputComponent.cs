using MH.EnitySystem;
using MH.GameState;
using UnityEngine;

namespace MH.Player
{
    public class PlayerInputComponent : EntityComponent
    {
        private IGameStateSystem gameStateSystem => ServiceLocator.Get<IGameStateSystem>();

        public override void ManualUpdate()
        {
            base.ManualUpdate();
        }
    }
}
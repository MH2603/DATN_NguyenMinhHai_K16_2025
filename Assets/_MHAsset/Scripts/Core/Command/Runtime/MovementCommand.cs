using UnityEngine;
using MH.EnitySystem;

namespace MH.Command.Runtime
{
    [CreateAssetMenu(fileName = "Move_Command", menuName = "MH_SO/Command/Movement Command")]
    public class MovementCommand : CommandSO
    {
        public Vector3 Direction = Vector3.zero;
        public float Speed = 100f;
        
        public override void Execute(BaseEnitity arg)
        {
            var movementComponent = arg.Get<MovementComponent>();
            if (movementComponent == null)
            {
                Debug.LogError("MovementCommand: MovementComponent is null");
                return; 
            }
            movementComponent.Move(Direction, Speed);
        }
    }
}
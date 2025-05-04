using MH.EnitySystem;
using UnityEngine;

namespace MH.Command
{
    public abstract class CommandSO : ScriptableObject, ICommand<BaseEnitity>
    {
        public abstract void Execute(BaseEnitity arg);


    }
}
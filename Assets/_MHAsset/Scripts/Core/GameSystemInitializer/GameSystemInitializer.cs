using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MH 
{ 
    public abstract class GameSystemInitializer : ScriptableObject
    {
        public virtual async UniTask Initialize()
        {

        }
        
    }
}

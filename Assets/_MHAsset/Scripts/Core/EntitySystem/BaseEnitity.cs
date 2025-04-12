

using System.Collections.Generic;
using UnityEngine;

namespace MH.EnitySystem
{
    public abstract class BaseEnitity : MonoBehaviour
    {
        [SerializeField] protected EntityComponent[] _components;

        protected Dictionary<System.Type, EntityComponent> _componentMap = new();

        private void Awake()
        {
            foreach (var component in _components) 
            {
                component.Initialized(this);

                _componentMap[component.GetType()] = component;
            }
        }

        public T Get<T>() where T : EntityComponent
        {
            if (_componentMap.ContainsKey(typeof(T)))
            {
                return _componentMap[typeof(T)] as T;    
            }

            return null;
        }
    }
}

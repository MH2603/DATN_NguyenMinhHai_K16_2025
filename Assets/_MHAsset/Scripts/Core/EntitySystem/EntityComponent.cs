using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MH.EnitySystem
{
    public abstract class EntityComponent : MonoBehaviour
    {
        protected BaseEnitity _entity;

        public virtual void Initialized(BaseEnitity baseEnitity)
        {
            _entity = baseEnitity;
        }

        public virtual void ManualUpdate() { }
        public virtual void ManualFixedUpdate() { }
    }
}

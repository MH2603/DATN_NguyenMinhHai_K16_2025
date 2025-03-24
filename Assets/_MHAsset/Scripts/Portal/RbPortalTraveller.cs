using System;
using UnityEngine;

namespace MH.Portal
{
    [RequireComponent(typeof(Rigidbody))]
    public class RbPortalTraveller : PortalTraveller
    {
        private Rigidbody _rb;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
            base.Teleport(fromPortal, toPortal, pos, rot);
            
            _rb.linearVelocity = toPortal.TransformVector (fromPortal.InverseTransformVector (_rb.linearVelocity));
            _rb.angularVelocity = toPortal.TransformVector (fromPortal.InverseTransformVector (_rb.angularVelocity));
            
            // _rb.linearVelocity = toPortal.TransformVector (fromPortal.TransformVector(_rb.linearVelocity));
            // _rb.angularVelocity = toPortal.TransformVector (fromPortal.TransformVector (_rb.angularVelocity));
        }
    }
}
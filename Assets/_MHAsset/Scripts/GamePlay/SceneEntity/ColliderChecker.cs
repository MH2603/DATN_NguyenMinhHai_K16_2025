using MH.EnitySystem;
using UnityEngine;
using UnityEngine.Events;

namespace MH
{
    [RequireComponent(typeof(BoxCollider))]
    public class ColliderChecker : EntityComponent
    {
        #region Fields
        [SerializeField] private LayerMask _objectLayer;           // Layer mask for detecting objects
        
        public UnityEvent OnEnterTrigger = new UnityEvent();      // Event when object is placed
        public UnityEvent OnExitTrigger = new UnityEvent();       // Event when object is removed
        
        private bool _isObjectOnTop;                               // Current state
        private Transform _transform;                              // Cached transform
        private BoxCollider _boxCollider;
        #endregion

        #region Unity Methods
        
        #endregion


        #region --------------- Public Methods ---------------

        public override void Initialized(BaseEnitity entity)
        {
            base.Initialized(entity);
            _transform = transform;
            _isObjectOnTop = false;
            _boxCollider = GetComponent<BoxCollider>();
        }
        
        public override void ManualUpdate()
        {
            base.ManualUpdate();
            // Check for object on top
            CheckObjectOnTop();
        }

        #endregion

        #region Private Methods
        private void CheckObjectOnTop()
        {
            // Cast a small box above the button
            Vector3 checkPosition = _transform.position + _boxCollider.center;
            Vector3 halfExtents = _boxCollider.size / 2f;
            
            bool objectDetected = Physics.OverlapBox(
                checkPosition,
                halfExtents,
                _transform.rotation,
                _objectLayer
            ).Length > 0;

            // Trigger events on state change
            if (objectDetected != _isObjectOnTop)
            {
                _isObjectOnTop = objectDetected;
                if (_isObjectOnTop)
                    OnEnterTrigger?.Invoke();
                else
                    OnExitTrigger?.Invoke();
            }
        }
        #endregion
    }
}
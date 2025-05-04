using MH.EnitySystem;
using UnityEngine;

namespace MH
{
    
    public class MovementComponent : EntityComponent
    {
        public float _finishDst = 0.05f;
        
        private Vector3 _startPos;
        private float _currentSpeed;
        private Vector3 _targetPosition;
        private Vector3 _targetDirection;
        private bool _isRunning;

        private Transform _trans;

        public override void Initialized(BaseEnitity entity)
        {
            base.Initialized(entity);
            
            _trans = entity.GetComponent<Transform>();
            _startPos = _trans.position;
        }

        public override void ManualFixedUpdate()
        {
            base.ManualFixedUpdate();
            
            if(_isRunning) MoveObjViaTransform();
        }
        
        public void Move(Vector3 direction, float speed)
        {
            _targetPosition = _startPos + direction;
            _currentSpeed = speed;
            CalculateTargetDirection();
            
            _isRunning = true;
        }

        public void MoveTo(Vector3 targetPosition, float speed)
        {
            _targetPosition = targetPosition;
            _currentSpeed = speed;
            CalculateTargetDirection();
            
            _isRunning = true;
            
        }

        private void CalculateTargetDirection()
        {
            _targetDirection = (_targetPosition - _trans.position).normalized;
        }


        private void MoveObjViaTransform()
        {
            float distance = Vector3.Distance(_targetPosition, _trans.position);
            if(TrackToEndRun(distance)) return;
            
            float deltaMoveDst = Mathf.Clamp(_currentSpeed * Time.fixedDeltaTime, 0, distance);
            
            _trans.position += _targetDirection * deltaMoveDst;
        }

        private bool TrackToEndRun(float currentDst)
        {
            if (currentDst <= _finishDst)
            {
                _isRunning = false;
                _trans.position = _targetPosition;
                
                return true;
            }

            return false;
        }
    }
}
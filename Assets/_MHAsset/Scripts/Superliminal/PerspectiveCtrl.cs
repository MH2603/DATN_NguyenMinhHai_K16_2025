using Cysharp.Threading.Tasks;
using MH.EnitySystem;
using MH.Interaction;
using UnityEngine;

namespace MH.Superliminal
{
    /// <summary>
    /// Controls perspective-based scaling and movement of objects in the game
    /// </summary>
    public class PerspectiveCtrl : EntityComponent
    {
        #region Serialized Fields
        [SerializeField] private float _checkDistance = 15f;    // Maximum distance to check for collisions
        [SerializeField] private float _minScale = 0.1f;        // Minimum allowed scale
        [SerializeField] private float _maxScale = 100f;        // Maximum allowed scale
        [SerializeField] private LayerMask _wallLayer;          // Layer mask for wall detection
        [SerializeField] private InteractableState State;       // Current interaction state
        #endregion

        #region Private Fields
        private float _startDragDistance;        // Initial distance when dragging starts
        private Vector3 _startDragSize;          // Initial size when dragging starts
        private float _currentDragDistance;      // Current distance while dragging
        private BoxCollider _collider;
        private Rigidbody _rb;
        private InteractableComponent _intertable;
        private IInteractor _interactor;            // Reference to the interactor
        private Transform _transform;

        // Computed properties
        private Vector3 _boxSize => _transform.localScale.x * _collider.size;
        private Vector3 _handDir => _transform.parent.forward;
        private Vector3 _handPos => _transform.parent.position;
        private Vector3 _targetPos;
        public Vector3 BoxSize { get; private set; }
        #endregion

        #region Entity Lifecycle
        public override void Initialized(BaseEnitity baseEnitity)
        {
            base.Initialized(baseEnitity);
            SetupComponents();
        }

        public override void ManualUpdate()
        {
            base.ManualUpdate();
            BoxSize = _boxSize;
            
            if (State == InteractableState.OnHand)
            {
                Dragging();
            }
        }
        #endregion

        #region Drag Operations
        public void StartDrag(Transform newParent = null)
        {
            if (!CanDrag()) return;

            SetDragState(newParent);
            UpdateDragParameters();
        }

        public async void EndDrag()
        {
            State = InteractableState.None;
            
            _transform.SetParent(null);
            _rb.isKinematic = false;
            _intertable.SetLockInteract(false);

            await UniTask.Delay(500);
            _interactor.IsActive = true;
            _interactor = null;
            
        }
        #endregion

        #region Private Methods
        private void SetupComponents()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
            _intertable = _entity.Get<InteractableComponent>();
            _transform = GetComponent<Transform>();
            
            State = InteractableState.None;
            _intertable.OnInteraction.AddListener(OnInteract);
        }

        private void SetDragState(Transform newParent)
        {
            State = InteractableState.OnHand;
            _transform.SetParent(newParent);
            _rb.isKinematic = true;
        }

        private void UpdateDragParameters()
        {
            _currentDragDistance = Vector3.Distance(_transform.position, _transform.parent.position);
            _startDragDistance = _currentDragDistance;
            _startDragSize = _transform.lossyScale;
        }

        private void OnInteract(IInteractor interactor)
        {
            if (!CanDrag()) return;

            _interactor = interactor;
            
            interactor.IsActive = false;
            StartDrag(interactor.Transform);
            _intertable.SetLockInteract(true);
            Debug.Log(" Start Drag !");
        }

        private bool CanDrag() => State == InteractableState.None;

        private void Dragging()
        {
            // Check for drag end input
            if (Input.GetKeyDown(KeyCode.E) && State == InteractableState.OnHand)
            {
                EndDrag();
                Debug.Log(" End Drag !");
                return;
            }

            UpdateTargetPosition();
            MovingToTargetPos(_targetPos);
        }

        private void UpdateTargetPosition()
        {
            float unitDistance = 0.02f;
            int loopAmount = (int)(_checkDistance / unitDistance);

            for (int i = 0; i < loopAmount; i++)
            {
                Vector3 checkPos = _handPos + _handDir * i * unitDistance;
                float checkScale = i * unitDistance / _startDragDistance;
                Vector3 checkHalfSize = _startDragSize / 2 * checkScale;

                if (Physics.OverlapBox(checkPos, checkHalfSize, _transform.rotation, _wallLayer).Length > 0)
                {
                    VisualDrawDebug.DisplayBox(checkPos, checkHalfSize, _transform.rotation, Color.yellow, 0.02f);
                    // MovingToTargetPos(checkPos);
                    _targetPos = checkPos;
                    break;
                }
            }
        }

        private void MovingToTargetPos(Vector3 newPosition)
        {
            // _transform.position = Vector3.Lerp(_transform.position, _targetPos, Time.deltaTime * 10f);
            _transform.position = newPosition;
            
            _currentDragDistance = Vector3.Distance(_transform.position, _transform.parent.position);
            _transform.localScale = _startDragSize * _currentDragDistance / _startDragDistance;
        }
        #endregion
    }
}
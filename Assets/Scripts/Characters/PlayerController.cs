using System;
using Characters.Moving;
using Characters.PlayerAttacks;
using Common;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(RotateController))]
    public class PlayerController : MonoBehaviour
    {
        public event Action BottleThrowStart;
        public event Action BottleThrowEnd;
        
        [SerializeField] private Transform bottlePlace;
        [SerializeField] private Bottle bottlePrefab;
        [SerializeField] private float maxThrowDistance;
        
        private MoveController _moveController; 
        private RotateController _rotateController;
        
        private Vector3 _lastTargetedPoint;
        private float _lastTargetedHeight;
        private bool _throwing;
        private Pool<Bottle> _bottlesPool;
        private void Start()
        {
            _moveController = GetComponent<MoveController>();
            _rotateController = GetComponent<RotateController>();
            _bottlesPool = new Pool<Bottle>(5, bottlePrefab);
            
            MouseHandler.GroundClick += MouseControllerOnGroundClick;
            _moveController.MoveTargetChanged += OnMoveTargetChanged;
        }

        private void MouseControllerOnGroundClick(RaycastHit hit)
        {
            if (_throwing)
            {
                return;
            }
            
            if (InputHandler.ShiftPressed)
            {
                _moveController.StopMovingCor();
                _rotateController.StartRotateCor(hit.point);

                var direction = hit.point - transform.position;
                var distance = direction.magnitude;
                
                if (distance > maxThrowDistance)
                {
                    _lastTargetedPoint = transform.position + direction.normalized * maxThrowDistance;
                    distance = maxThrowDistance;
                }
                else
                {
                    _lastTargetedPoint = hit.point;
                }
                
                _lastTargetedHeight = .25f * distance;
                
                ThrowBottle();
            }
            else
            {
                if (hit.collider && hit.collider.CompareTag("WalkableArea"))
                {
                    _moveController.StartMovingCor(hit.point);
                }
            }
        }

        private void OnMoveTargetChanged(Vector3 pos)
        {
            _rotateController.RotateTo(pos);
        }
        
        private void ThrowBottle()
        {
            _throwing = true;
            BottleThrowStart?.Invoke();
        }

        /// <summary>
        /// Calls from animation event
        /// </summary>
        public void ThrowMoment()
        {
            var bottle = _bottlesPool.Get(bottlePlace.position, bottlePlace.rotation);
            
            bottle.Launch(_lastTargetedPoint, _lastTargetedHeight);
            BottleThrowEnd?.Invoke();
            
            _throwing = false;
        }
    }
}
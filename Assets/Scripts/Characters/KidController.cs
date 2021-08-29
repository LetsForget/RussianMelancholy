using System;
using Characters.Animation;
using Characters.Moving;
using Characters.PlayerAttacks;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(RagdollSwitcher))]
    public class KidController : MonoBehaviour
    {
        public event Action StartAbuse;
        public event Action StopAbuse;
        
        [SerializeField] private Transform abuseTarget;
        
        [SerializeField] private float stayDistance;
        [SerializeField] private float runToDistance;
        
        private bool _isRunningTo;
        private bool _isAbusing;
        
        private MoveController _moveController;
        private RotateController _rotateController;
        private RagdollSwitcher _ragdollSwitcher;
        
        private bool IsAbusing
        {
            get => _isAbusing;
            set
            {
                _isAbusing = value;
                
                if (value)
                {
                    StartAbuse?.Invoke();
                }
                else
                {
                    StopAbuse?.Invoke();
                }
            }
        }
        
        private void Start()
        {
            _moveController = GetComponent<MoveController>();
            _rotateController = GetComponent<RotateController>();
            _ragdollSwitcher = GetComponent<RagdollSwitcher>();
            
            _isRunningTo = false;
            _ragdollSwitcher.RagdollEnable = false;
        }

        private void Update()
        {
            var targetPosition = abuseTarget.position;
            
            _rotateController.RotateTo(targetPosition);
            
            var distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            
            if (!_isRunningTo && distanceToTarget > stayDistance)
            {
                _isRunningTo = true;
            }
            else if (_isRunningTo && distanceToTarget < runToDistance)
            {
                _isRunningTo = false;
            }

            if (distanceToTarget < stayDistance && !IsAbusing)
            {
                IsAbusing = true;
            }
            else if (distanceToTarget > stayDistance && IsAbusing)
            {
                IsAbusing = false;
            }
            
            if (_isRunningTo)
            {
                _moveController.MoveTo(targetPosition);
            }
        }

        public void AffectByBottle()
        {
            _ragdollSwitcher.RagdollEnable = true;
            enabled = false;
        }

        public Rigidbody[] GetRagdollRigidbodies()
        {
            return _ragdollSwitcher.RagdollBodies;
        }

        private void OnDestroy()
        {
            StartAbuse = null;
            StopAbuse = null;
        }
    }
}
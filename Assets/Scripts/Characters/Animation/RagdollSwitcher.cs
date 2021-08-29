using System;
using UnityEngine;

namespace Characters.Animation
{
    public class RagdollSwitcher : MonoBehaviour
    {
        [SerializeField] private Transform ragdollHolder;

        private Animator _animator;
        
        private Rigidbody _mainRigidbody;
        private CapsuleCollider _mainCollider;
        
        private Rigidbody[] _rigidbodies;
        private Collider[] _colliders;
        
        public bool RagdollEnable
        {
            set
            {
                _mainRigidbody.isKinematic = value;
                _mainCollider.enabled = !value;
                
                foreach (var collider in _colliders)
                {
                    collider.enabled = value;
                }
                
                foreach (var rigidbody in _rigidbodies)
                {
                    rigidbody.isKinematic = !value;
                }

                if (_animator)
                {
                    _animator.enabled = !value;
                }
            }
        }
        public Rigidbody[] RagdollBodies => _rigidbodies;
        
        private void Awake()
        {
            _mainRigidbody = GetComponent<Rigidbody>();
            _mainCollider = GetComponent<CapsuleCollider>();
            
            _rigidbodies = ragdollHolder.GetComponentsInChildren<Rigidbody>();
            _colliders = ragdollHolder.GetComponentsInChildren<Collider>();
            
            _animator = GetComponent<Animator>();
        }
    }
}
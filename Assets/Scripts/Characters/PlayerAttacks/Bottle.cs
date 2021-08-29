using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.PlayerAttacks
{
    public class Bottle : MonoBehaviour
    {
        [SerializeField] private BottleCharacteristics characteristics;
        
        private bool _exploded;
        private Rigidbody _rigidbody;

        public void Launch(Vector3 target, float h)
        {
            _exploded = false;
            
            var currentPos = transform.position;
            var gravity = Physics.gravity.y;

            float displacementY = target.y - currentPos.y;
            Vector3 displacementXZ = new Vector3(target.x - currentPos.x, 0, target.z - currentPos.z);

            float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
            Vector3 velocityXZ = displacementXZ / time;

            var launchVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
            
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.velocity = launchVelocity;
            _rigidbody.AddTorque(Vector3.left);
        }

        private IEnumerator OnCollisionEnter(Collision other)
        {
            if (other.impulse.magnitude < characteristics.ImpulseToExplode 
                || _exploded)
            {
                yield break;
            }
            
            _exploded = true;

            var explosionPos = transform.position;
            var colliders = Physics.OverlapSphere(explosionPos, characteristics.ExplosionRadius);
            var bodies = new HashSet<Rigidbody>();
            
            foreach (var coll in colliders)
            {
                var collRbody = coll.GetComponent<Rigidbody>();
                if (collRbody)
                {
                    bodies.Add(collRbody);
                }

                
                var kid = coll.GetComponent<KidController>();
                if (!kid)
                {
                    continue;
                }
                
                kid.AffectByBottle();
                var kidBodies = kid.GetRagdollRigidbodies();
                
                foreach (var body in kidBodies)
                {
                    bodies.Add(body);
                }
            }

            yield return new WaitForFixedUpdate();
            
            foreach (var body in bodies)
            {
                body.AddExplosionForce(characteristics.ExplosionForce, explosionPos, characteristics.ExplosionRadius);
            }
        }
    }
}
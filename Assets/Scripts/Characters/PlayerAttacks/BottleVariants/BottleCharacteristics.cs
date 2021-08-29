using UnityEngine;

namespace Characters.PlayerAttacks
{
    [CreateAssetMenu(fileName = "New BottleCharacteristics", menuName = "BottleCharacteristics", order = 51)]
    public class BottleCharacteristics : ScriptableObject
    {
        public float ExplosionForce => explosionForce;
        [SerializeField] private float explosionForce;
        
        public float ExplosionRadius => explosionRadius;
        [SerializeField] private float explosionRadius;

        public float ImpulseToExplode => impulseToExplode;
        [SerializeField] private float impulseToExplode;
    }
}

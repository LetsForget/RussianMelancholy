using UnityEngine;

namespace Characters.Moving
{
    [CreateAssetMenu(fileName = "New MoveParameters", menuName = "MoveParameters", order = 51)]
    public class MoveParameters : ScriptableObject
    {
        public float MaxSpeed => maxSpeed;
        [SerializeField] private float maxSpeed;

        public float Acceleration => acceleration;
        [SerializeField] private float acceleration;
        
        public float Deceleration => deceleration;
        [SerializeField] private float deceleration;
        
        public float TurnSpeed => turnSpeed;
        [SerializeField] private float turnSpeed;
    }
}
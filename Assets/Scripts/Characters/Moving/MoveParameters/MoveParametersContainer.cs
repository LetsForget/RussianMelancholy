using UnityEngine;

namespace Characters.Moving
{
    public class MoveParametersContainer : MonoBehaviour
    {
        [SerializeField] private MoveParameters moveParameters;
        
        public float MaxSpeed => moveParameters.MaxSpeed;
        public float Acceleration => moveParameters.Acceleration;
        public float Deceleration => moveParameters.Deceleration;

        public float TurnSpeed => moveParameters.TurnSpeed;
    }
}
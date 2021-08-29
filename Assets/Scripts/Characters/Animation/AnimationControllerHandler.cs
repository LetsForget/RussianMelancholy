using Characters.Moving;
using UnityEngine;

namespace Characters.Animation
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(RotateController))]
    public class AnimationControllerHandler : MonoBehaviour
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Turn = Animator.StringToHash("Turn");
        
        protected Animator Animator;
        
        private MoveController _moveController;
        private RotateController _rotateController;
        private MoveParametersContainer _moveParameters;
        
        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            _moveController = GetComponent<MoveController>();
            _rotateController = GetComponent<RotateController>();
            _moveParameters = GetComponent<MoveParametersContainer>();
            
            _moveController.SpeedChanged += OnSpeedChanged;
            _rotateController.TurnChanged += OnTurnChanged;
        }
        
        private void OnSpeedChanged(Vector3 speed)
        {
            var value = speed.magnitude / _moveParameters.MaxSpeed;
            
            if (value > 1)
            {
                value = 1;
            }
            
            Animator.SetFloat(Move, value);
        }
        
        private void OnTurnChanged(float value)
        {
           // _animator.SetFloat(Turn, value);
        }
    }
}
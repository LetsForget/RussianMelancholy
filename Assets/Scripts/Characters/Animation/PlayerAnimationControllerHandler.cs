using UnityEngine;

namespace Characters.Animation
{
    public class PlayerAnimationControllerHandler : AnimationControllerHandler
    {
        private static readonly int Throwing = Animator.StringToHash("Throwing");
        
        private PlayerController _playerController;

        protected override void Start()
        {
            base.Start();
            
            _playerController = GetComponent<PlayerController>();
            _playerController.BottleThrowStart += OnBottleThrowStart;
            _playerController.BottleThrowEnd += OnStopThrowing;
        }

        private void OnBottleThrowStart()
        {
            Animator.SetBool(Throwing, true);
        }

        private void OnStopThrowing()
        {
            Animator.SetBool(Throwing, false);
        }
    }
}
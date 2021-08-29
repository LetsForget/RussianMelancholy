using UnityEngine;

namespace Characters.Animation
{
    [RequireComponent(typeof(KidController))]
    public class KidAnimationControllerHandler : AnimationControllerHandler
    {
        private static readonly int Abuse = Animator.StringToHash("Abuse");
        private static readonly int StopAbuse = Animator.StringToHash("StopAbuse");
        
        private KidController _kidController;
        
        protected override void Start()
        {
            base.Start();
            _kidController = GetComponent<KidController>();
            
            _kidController.StartAbuse += OnStartAbuse;
            _kidController.StopAbuse += OnStopAbuse;
        }
        
        private void OnStartAbuse()
        {
            Animator.SetTrigger(Abuse);
        }
        
        private void OnStopAbuse()
        {
            Animator.SetTrigger(StopAbuse);
        }
    }
}
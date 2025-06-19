using UnityEngine;

namespace IdleProject.Character.Character
{
    public class CharacterAnimationController
    {
        protected readonly Animator _animator;

        public CharacterAnimationController(Animator animator)
        {
            _animator = animator;
        }
        
        public void SetAnimationController(RuntimeAnimatorController animationController)
        {
            _animator.runtimeAnimatorController = animationController;
            _animator.Rebind();
        }
        
        protected void ResetAnimation()
        {
            foreach (var trigger in _animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    _animator.ResetTrigger(trigger.name);
                }
            }
        }
    }
}
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class AnimationController
    {
        public AnimationEventHandler AnimEventHandler { get; private set; }

        private Animator animator;

        private readonly int SkillAnimHash = Animator.StringToHash("Skill");
        private readonly int AttackAnimHash = Animator.StringToHash("Attack");
        private readonly int DeathAnimHash = Animator.StringToHash("Death");
        private readonly int MoveAnimHash = Animator.StringToHash("Move");
        private readonly int IdleAnimHash = Animator.StringToHash("Idle");
        private readonly int WinAnimHash = Animator.StringToHash("Win");

        public AnimationController(Animator animator, AnimationEventHandler animEventHandler)
        {
            this.animator = animator;
            AnimEventHandler = animEventHandler;
        }

        public void SetSkill()
        {
            animator.SetTrigger(SkillAnimHash);
        }

        public void SetAttack()
        {
            ResetAnimation();
            animator.SetTrigger(AttackAnimHash);
        }

        public void SetDeath()
        {
            ResetAnimation();
            animator.SetTrigger(DeathAnimHash);
        }

        public void SetMove()
        {
            animator.SetTrigger(MoveAnimHash);
        }

        public void SetIdle()
        {
            animator.SetTrigger(IdleAnimHash);
        }

        public void SetWin()
        {
            ResetAnimation();
            animator.SetTrigger(WinAnimHash);
        }

        private void ResetAnimation()
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
        }
    }
}
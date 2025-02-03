using UnityEngine;

namespace IdleProject.Battle.Character
{
    public class AnimationController
    {
        public AnimationEventHandler AnimEventHandler { get; private set; }

        private Animator animator;

        private readonly int skillAnimHash = Animator.StringToHash("Skill");
        private readonly int attackAnimHash = Animator.StringToHash("Attack");
        private readonly int deathAnimHash = Animator.StringToHash("Death");
        private readonly int moveAnimHash = Animator.StringToHash("Move");
        private readonly int idleAnimHash = Animator.StringToHash("Idle");

        public AnimationController(Animator animator, AnimationEventHandler animEventHandler)
        {
            this.animator = animator;
            AnimEventHandler = animEventHandler;
        }

        public void SetSkill()
        {
            animator.SetTrigger(skillAnimHash);
        }

        public void SetAttack()
        {
            animator.SetTrigger(attackAnimHash);
        }

        public void SetDeath()
        {
            animator.SetTrigger(deathAnimHash);
        }

        public void SetMove()
        {
            animator.SetTrigger(moveAnimHash);
        }

        public void SetIdle()
        {
            animator.SetTrigger(idleAnimHash);
        }
    }
}
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public delegate void AnimationEventHandle();

    public class AnimationEventHandler : MonoBehaviour
    {
        public event AnimationEventHandle AttackStartEvent;
        public event AnimationEventHandle AttackEndEvent;
        public event AnimationEventHandle AttackHitEvent;
        public event AnimationEventHandle DeathEndEvent;
        public event AnimationEventHandle SkillEvent;
        public event AnimationEventHandle SkillStartEvent;
        public event AnimationEventHandle SkillEndEvent;

        private void AttackStart()
        {
            AttackStartEvent?.Invoke();
        }

        private void AttackEnd()
        {
            AttackEndEvent?.Invoke();
        }

        private void AttackHit()
        {
            AttackHitEvent?.Invoke();
        }

        private void DeathEnd()
        {
            DeathEndEvent?.Invoke();
        }

        private void Skill()
        {
            SkillEvent?.Invoke();
        }

        private void SkillStart()
        {
            SkillStartEvent?.Invoke();
        }

        private void SkillEnd()
        {
            SkillEndEvent?.Invoke();
        }
    }
}
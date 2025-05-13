using UnityEngine;

namespace IdleProject.Battle.Character
{
    public delegate void AnimationEventHandle();

    public class AnimationEventHandler : MonoBehaviour
    {
        public event AnimationEventHandle AttackStartEvent;
        public event AnimationEventHandle AttackEndEvent;
        public event AnimationEventHandle AttackEvent;
        public event AnimationEventHandle DeathEndEvent;
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

        private void Attack()
        {
            AttackEvent?.Invoke();
        }

        private void DeathEnd()
        {
            DeathEndEvent?.Invoke();
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
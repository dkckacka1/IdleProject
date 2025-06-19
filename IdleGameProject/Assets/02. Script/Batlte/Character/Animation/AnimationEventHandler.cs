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
        public event AnimationEventHandle SkillFirstEvent;
        public event AnimationEventHandle SkillSecondEvent;
        public event AnimationEventHandle SkillThirdEvent;
        public event AnimationEventHandle SkillFourthEvent;
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

        private void SkillHitFirst()
        {
            SkillFirstEvent?.Invoke();
        }

        private void SkillHitSecond()
        {
            SkillSecondEvent?.Invoke();
        }

        private void SkillHitThird()
        {
            SkillThirdEvent?.Invoke();
        }

        private void SkillHitFourth()
        {
            SkillFourthEvent?.Invoke();
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
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.Character
{

    public class AnimationEventHandler : MonoBehaviour
    {
        public event UnityAction AttackStartEvent;
        public event UnityAction AttackEndEvent;
        public event UnityAction DeathEndEvent;
        public event UnityAction SkillStartEvent;
        public event UnityAction SkillEndEvent;
        public event UnityAction<int> AttackActionEvent;
        public event UnityAction<string> AttackEffectEvent;
        public event UnityAction<int> SkillActionEvent;
        public event UnityAction<string> SkillEffectEvent;

        private void AttackAction(int attackNumber)
        {
            AttackActionEvent?.Invoke(attackNumber);
        }

        private void AttackEffectPlay(string effectName)
        {
            AttackEffectEvent?.Invoke(effectName);
        }

        private void SkillAction(int skillNumber)
        {
            SkillActionEvent?.Invoke(skillNumber);
        }

        private void SkillEffectPlay(string effectName)
        {
            SkillEffectEvent?.Invoke(effectName);
        }

        private void AttackStart()
        {
            AttackStartEvent?.Invoke();
        }

        private void AttackEnd()
        {
            AttackEndEvent?.Invoke();
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
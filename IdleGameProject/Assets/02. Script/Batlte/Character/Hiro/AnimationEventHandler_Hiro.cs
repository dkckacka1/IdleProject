using UnityEngine;

namespace IdleProject.Battle.Character.Hiro
{
    public class AnimationEventHandler_Hiro : AnimationEventHandler
    {
        public event AnimationEventHandle AttackEvent;
        public event AnimationEventHandle SkillEvent;

        private void Attack()
        {
            AttackEvent?.Invoke();
        }

        private void Skill()
        {
            SkillEvent?.Invoke();
        }
    }
}
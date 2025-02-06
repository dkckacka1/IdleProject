using UnityEngine;

namespace IdleProject.Battle.Character.Hiro
{
    public class AnimationEventHandler_Hiro : AnimationEventHandler
    {
        public event AnimationEventHandle SkillEvent;

        private void Skill()
        {
            SkillEvent?.Invoke();
        }
    }
}
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public abstract class CharacterSkill
    {
        public CharacterController controller;

        public abstract void SetAnimationEvent(AnimationEventHandler eventHandler);
    }
}
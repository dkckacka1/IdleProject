using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public abstract class CharacterSkill
    {
        public CharacterController Controller;

        public abstract void SetAnimationEvent(AnimationEventHandler eventHandler);
    }
}
using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public interface ISkillTargeting
    {
        public bool TargetingCharacterList(CharacterController compareTarget, CharacterController currentTarget = null);
    }
}
using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public interface ISkillTargeting
    {
        public IEnumerable<CharacterController> TargetingCharacterList(List<CharacterController> compareTargetList,
            CharacterController currentTarget = null);
    }
}
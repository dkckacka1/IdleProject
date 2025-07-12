using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public interface ISkillTargeting
    {
        public List<CharacterController> GetTargetingCharacterList
        (
            CharacterController targetCharacter,
            List<CharacterController> allCharacterList,
            List<CharacterController> currentTargetList
        );
    }
}
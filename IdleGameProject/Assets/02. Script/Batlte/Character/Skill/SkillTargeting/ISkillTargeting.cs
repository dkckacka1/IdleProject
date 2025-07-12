using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public interface ISkillTargeting
    {
        public List<CharacterController> GetTargetingCharacterList
        (
            CharacterController userCharacter,
            List<CharacterController> allCharacterList,
            List<CharacterController> currentTargetList
        );
    }
}
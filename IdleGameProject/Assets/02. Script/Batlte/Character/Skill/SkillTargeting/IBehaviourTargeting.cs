using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public interface IBehaviourTargeting
    {
        public List<CharacterController> GetTargetingCharacterList
        (
            CharacterController targetCharacter,
            List<CharacterController> allCharacterList,
            List<CharacterController> currentTargetList
        );
    }
}
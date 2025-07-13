using System.Collections.Generic;

namespace IdleProject.Battle.Character.Behaviour.Targeting
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
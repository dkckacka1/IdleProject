using System.Collections.Generic;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public abstract class BehaviourTargeting : IBehaviourTargeting
    {
        private readonly bool _isCheckFromTarget;
        protected readonly CharacterController UseSkillController;

        protected List<CharacterController> GetCheckList(List<CharacterController> allCharacterList,
            List<CharacterController> targetList) => _isCheckFromTarget ? targetList : allCharacterList;

        protected BehaviourTargeting(CharacterController useSkillController, SkillTargetingData targetingDataData)
        {
            UseSkillController = useSkillController;
            _isCheckFromTarget = targetingDataData.isCheckFromTarget;
        }

        public static IBehaviourTargeting GetSkillTargeting(CharacterController useSkillController, SkillTargetingData targetingData)
        {
            return targetingData switch
            {
                AITargetingData aiTargeting => new Implement.AITargeting(useSkillController, aiTargeting),
                CharacterStateTargetingData characterStateTargetingData => new Implement.CharacterStateTargeting(useSkillController, characterStateTargetingData),
                RangeTargetingData rangeTargeting => new Implement.RangeTargeting(useSkillController, rangeTargeting),
                SingleTargetingData singleTargeting => new Implement.SingleTargeting(useSkillController, singleTargeting),
                _ => null
            };
        }

        public abstract List<CharacterController> GetTargetingCharacterList(CharacterController targetCharacter, List<CharacterController> allCharacterList, List<CharacterController> currentTargetList);
    }
}
using System.Collections.Generic;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.Targeting
{
    public abstract class BehaviourTargeting : IBehaviourTargeting
    {
        private readonly bool _isCheckFromTarget;
        protected readonly CharacterController UseSkillController;

        protected List<CharacterController> GetCheckList(List<CharacterController> allCharacterList,
            List<CharacterController> targetList) => _isCheckFromTarget ? targetList : allCharacterList;

        protected BehaviourTargeting(CharacterController useSkillController, BehaviourTargetingData targetingDataData)
        {
            UseSkillController = useSkillController;
            _isCheckFromTarget = targetingDataData.isCheckFromTarget;
        }

        public static IBehaviourTargeting GetSkillTargeting(CharacterController useSkillController, BehaviourTargetingData targetingData)
        {
            return targetingData switch
            {
                // 대상 AI 타겟팅
                AITargetingData aiTargeting => new Implement.AITargeting(useSkillController, aiTargeting),
                // 대상 상태 타겟팅
                CharacterStateTargetingData characterStateTargetingData => new Implement.CharacterStateTargeting(useSkillController, characterStateTargetingData),
                // 대상 사거리 타겟팅
                RangeTargetingData rangeTargeting => new Implement.RangeTargeting(useSkillController, rangeTargeting),
                // 한명만 타겟팅
                SingleTargetingData singleTargeting => new Implement.SingleTargeting(useSkillController, singleTargeting),
                _ => null
            };
        }

        public abstract List<CharacterController> GetTargetingCharacterList(CharacterController targetCharacter, List<CharacterController> allCharacterList, List<CharacterController> currentTargetList);
    }
}
using System.Collections.Generic;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillTargeting
{
    public abstract class SkillTargeting : ISkillTargeting
    {
        private readonly bool _isCheckFromTarget;
        private readonly CharacterController _useSkillController;

        protected CharacterController GetCheckController(CharacterController target) =>
            target is not null && _isCheckFromTarget ? target : _useSkillController;

        protected SkillTargeting(CharacterController useSkillController, SkillTargetingData targetingDataData)
        {
            _useSkillController = useSkillController;
            _isCheckFromTarget = targetingDataData.isCheckFromTarget;
        }

        public static ISkillTargeting GetSkillTargeting(CharacterController useSkillController, SkillTargetingData targetingData)
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
        
        public abstract IEnumerable<CharacterController> TargetingCharacterList(List<CharacterController> compareTargetList,
            CharacterController currentTarget = null);
    }
}
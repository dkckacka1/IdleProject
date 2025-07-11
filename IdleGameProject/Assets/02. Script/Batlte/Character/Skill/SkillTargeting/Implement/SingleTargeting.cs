using System;
using IdleProject.Data.StaticData.Skill;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class SingleTargeting : SkillTargeting
    {
        private SingleTargetingData.SingleTargetType _targetType; 
        
        public SingleTargeting(CharacterController useSkillController, SingleTargetingData targetingDataData) : base(useSkillController, targetingDataData)
        {
            _targetType = targetingDataData.singleTargetType;
        }

        public override bool TargetingCharacterList(CharacterController compareTarget, CharacterController currentTarget = null)
        {
            return _targetType switch
            {
                SingleTargetingData.SingleTargetType.Self => CheckController(currentTarget),
                SingleTargetingData.SingleTargetType.NealyController => CheckController(currentTarget),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
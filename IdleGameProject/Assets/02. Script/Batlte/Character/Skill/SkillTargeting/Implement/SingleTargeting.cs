using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.SkillData;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class SingleTargeting : SkillTargeting
    {
        private SingleTargetingData.SingleTargetType _targetType; 
        
        public SingleTargeting(CharacterController useSkillController, SingleTargetingData targetingDataData) : base(useSkillController, targetingDataData)
        {
            _targetType = targetingDataData.singleTargetType;
        }

        public override IEnumerable<CharacterController> TargetingCharacterList(List<CharacterController> compareTargetList,
            CharacterController currentTarget = null)
        {
            var checkController = GetCheckController(currentTarget);
            
            return _targetType switch
            {
                SingleTargetingData.SingleTargetType.Self => new List<CharacterController> {GetCheckController(currentTarget)},
                SingleTargetingData.SingleTargetType.NealyController => compareTargetList.OrderBy(target => Vector3.Distance(target, checkController)).Take(1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
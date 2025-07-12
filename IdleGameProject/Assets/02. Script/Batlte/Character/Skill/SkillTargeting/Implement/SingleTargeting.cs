using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.SkillData;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class SingleTargeting : SkillTargeting
    {
        private readonly SingleTargetingData.SingleTargetType _targetType;

        public SingleTargeting(CharacterController useSkillController, SingleTargetingData targetingDataData) : base(
            useSkillController, targetingDataData)
        {
            _targetType = targetingDataData.singleTargetType;
        }


        public override List<CharacterController> GetTargetingCharacterList(CharacterController userCharacter,
            List<CharacterController> allCharacterList, List<CharacterController> currentTargetList)
        {
            var checkList = GetCheckList(allCharacterList, currentTargetList);

            return _targetType switch
            {
                SingleTargetingData.SingleTargetType.Self => new List<CharacterController> { userCharacter },
                SingleTargetingData.SingleTargetType.NealyController => checkList
                    .OrderBy(character => Vector3.Distance(character, UseSkillController)).Take(1).ToList(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
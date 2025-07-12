using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.SkillData;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class RangeTargeting : SkillTargeting
    {
        private readonly float _range;

        public RangeTargeting(CharacterController useSkillController, RangeTargetingData targetingDataData) : base(
            useSkillController, targetingDataData)
        {
            _range = targetingDataData.skillRange;
        }

        public override IEnumerable<CharacterController> TargetingCharacterList(
            List<CharacterController> compareTargetList,
            CharacterController currentTarget = null)
        {
            var checkController = GetCheckController(currentTarget);

            return compareTargetList.Where(target => Vector3.Distance(target, checkController) < _range);
        }
    }
}
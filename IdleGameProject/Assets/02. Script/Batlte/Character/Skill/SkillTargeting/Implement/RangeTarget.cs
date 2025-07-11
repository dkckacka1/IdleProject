using System.Collections.Generic;
using IdleProject.Data.StaticData.Skill;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class RangeTarget : SkillTargeting

    {
        public RangeTarget(CharacterController useSkillController, SkillTargetingData targetingDataData) : base(useSkillController, targetingDataData)
        {
        }

        public override bool TargetingCharacterList(CharacterController compareTarget, CharacterController currentTarget = null)
        {
            // TODO 
            return false;
        }
    }
}
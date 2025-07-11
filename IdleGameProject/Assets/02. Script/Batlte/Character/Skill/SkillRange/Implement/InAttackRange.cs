using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillRange.Implement
{
    public class InAttackRange : SkillRange
    {
        public InAttackRange(CharacterController controller) : base(controller)
        {
        }

        public override bool GetInRange(CharacterController target)
        {
            return Vector3.Distance(target, Controller) <
                   Controller.StatSystem.GetStatValue(CharacterStatType.AttackRange);
        }
    }
}
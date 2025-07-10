using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillRange.Implement
{
    public class InAttackRange : ISkillRange
    {
        private CharacterController _controller;

        public InAttackRange(CharacterController controller)
        {
            _controller = controller;
        }

        public bool GetInRange(CharacterController target)
        {
            return Vector3.Distance(target, _controller) <
                   _controller.StatSystem.GetStatValue(CharacterStatType.AttackRange);
        }
    }
}
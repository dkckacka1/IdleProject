using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillRange.Implement
{
    public class TargetRange : SkillRange
    {
        private readonly float _skillRange;

        public TargetRange(CharacterController controller, float skillRange) : base(controller)
        {
            _skillRange = skillRange;
        }

        public override bool GetInRange(CharacterController target)
        {
            return Vector3.Distance(target, Controller.GetTargetCharacter.Invoke()) < _skillRange;
        }
    }
}
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillRange.Implement
{
    public class SelfRange : ISkillRange
    {
        private readonly float _skillRange;
        private readonly CharacterController _controller;

        public SelfRange(CharacterController controller, float skillRange)
        {
            _controller = controller;
            _skillRange = skillRange;
        }

        public bool GetInRange(CharacterController target)
        {
            return Vector3.Distance(target, _controller) < _skillRange;
        }
    }
}
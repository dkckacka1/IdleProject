using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTarget.Implement
{
    public class CurrentTargeting : ISkillGetTarget
    {
        public List<CharacterController> GetTargetList(CharacterController controller) => new() {controller.GetTargetCharacter.Invoke()};
    }
}
using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public interface ISkillAction
    {
        public void ExecuteSkillAction(CharacterController controller, IEnumerable<CharacterController> targetList);
    }
}
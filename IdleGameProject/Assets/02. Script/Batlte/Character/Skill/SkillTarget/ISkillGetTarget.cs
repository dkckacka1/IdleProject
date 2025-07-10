
using System.Collections.Generic;

namespace IdleProject.Battle.Character.Skill.SkillTarget
{
    public interface ISkillGetTarget
    {
        List<CharacterController> GetTargetList(CharacterController controller);
    }
}

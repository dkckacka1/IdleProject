using System.Collections.Generic;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill.SkillRange
{
    public interface ISkillRange
    {
        public bool GetInRange(CharacterController target);
    }
}
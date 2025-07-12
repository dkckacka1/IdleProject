using System.Collections.Generic;
using IdleProject.Battle.Character.Skill.SkillAction;
using UnityEngine;

namespace IdleProject.Data.SkillData
{
    public class SkillExecuteData : ScriptableObject
    {
        [SerializeReference]
        public List<SkillActionData> skillActionDataList = new();
    }
}
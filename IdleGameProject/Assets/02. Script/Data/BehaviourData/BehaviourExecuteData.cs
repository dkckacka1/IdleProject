using System.Collections.Generic;
using IdleProject.Battle.Character.Behaviour.SkillAction;
using UnityEngine;

namespace IdleProject.Data.BehaviourData
{
    public class BehaviourExecuteData : ScriptableObject
    {
        [SerializeReference]
        public List<BehaviourActionData> skillActionDataList = new();
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Data.BehaviourData
{
    public class BehaviourExecuteData : ScriptableObject
    {
        [SerializeReference]
        public List<BehaviourActionData> skillActionDataList = new();
    }
}
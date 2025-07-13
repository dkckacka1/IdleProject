using System.Collections.Generic;
using IdleProject.Data.BehaviourData;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using IdleProject.EditorClass;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
    public class StaticSkillData : StaticData
    {
        public string skillName;
        public string skillDesc;

        [FormerlySerializedAs("actionDataList")] [SerializeReference] public List<BehaviourExecuteData> executeDataList = new List<BehaviourExecuteData>();

#if UNITY_EDITOR
        [Button]
        private void AddSkillExecuteData()
        {
            ScriptableObjectUtil.AddChildObject(this, executeDataList, num => $"{name}_{num}");
        }

        [Button]
        private void RefreshExecuteData()
        {
            ScriptableObjectUtil.RefreshChildList(this, executeDataList, num => $"{name}_{num}");
        }
#endif
    }
}
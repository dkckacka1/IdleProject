using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Data.SerializableData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StaticStageData : StaticData
    {
        public int chapterIndex = 1;
        public int stageIndex = 1;
        public string stageName;
        
        public FormationInfo stageFormation;

        public int playerExpAmount;

        public List<RewardData> rewardDataList;
        
        public override string Index => $"{chapterIndex}-{stageIndex}";
    }
}

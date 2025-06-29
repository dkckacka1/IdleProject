using IdleProject.Core;
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

        public override string Index => $"{chapterIndex}-{stageIndex}";
    }
}

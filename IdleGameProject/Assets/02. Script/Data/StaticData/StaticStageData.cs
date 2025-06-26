using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StaticStageData : StaticData
    {
        public FormationInfo stageFormation;
    }
}

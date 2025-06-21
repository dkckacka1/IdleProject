using UnityEngine;

namespace IdleProject.Data
{
    [System.Serializable]
    public struct FormationInfo
    {
        public string frontMiddleCharacterName;
        public string frontRightCharacterName;
        public string frontLeftCharacterName;
        public string rearRightCharacterName;
        public string rearLeftCharacterName;
    }
    
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StageData : Data
    {
        public int stageNumber;
        public FormationInfo stageFormation;

        public override string Index => stageNumber.ToString();
    }

}

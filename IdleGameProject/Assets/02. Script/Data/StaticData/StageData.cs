using System.Collections.Generic;
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

        public List<string> GetCharacterNameList()
        {
            var result = new List<string>();
            AddString(frontMiddleCharacterName);
            AddString(frontRightCharacterName);
            AddString(frontLeftCharacterName);
            AddString(rearRightCharacterName);
            AddString(rearLeftCharacterName);

            return result;
            
            void AddString(string name)
            {
                if (string.IsNullOrEmpty(name) is false)
                {
                    result.Add(name);
                }
            }
        }
    }
    
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StageData : Data
    {
        public int stageNumber;
        public FormationInfo stageFormation;

        public override string Index => stageNumber.ToString();
    }

}

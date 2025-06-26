using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Data.StaticData
{
    [System.Serializable]
    public struct PositionInfo
    {
        public string characterName;
        public int characterLevel;
    }
    
    [System.Serializable]
    public struct FormationInfo
    {
        public PositionInfo frontMiddlePositionInfo;
        public PositionInfo frontRightPositionInfo;
        public PositionInfo frontLeftPositionInfo;
        public PositionInfo rearRightPositionInfo;
        public PositionInfo rearLeftPositionInfo;

        public List<string> GetCharacterNameList()
        {
            var result = new List<string>();
            AddString(frontMiddlePositionInfo);
            AddString(frontRightPositionInfo);
            AddString(frontLeftPositionInfo);
            AddString(rearRightPositionInfo);
            AddString(rearLeftPositionInfo);

            return result;
            
            void AddString(PositionInfo info)
            {
                if (string.IsNullOrEmpty(info.characterName) is false)
                {
                    result.Add(info.characterName);
                }
            }
        }
    }
    
    [CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
    public class StaticStageData : StaticData
    {
        public FormationInfo stageFormation;
    }
}

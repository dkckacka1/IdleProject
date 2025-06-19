using IdleProject.Battle.Spawn;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Core.GameData
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

    // 씬 간 데이터 보존 클래스
    [System.Serializable]
    public class DataController
    {
        public FormationInfo playerFormationInfo;
        public FormationInfo enemyFormationInfo;

        public DataController()
        {
            
        }

        public DataController(TestStaticData staticData)
        {
            playerFormationInfo = staticData.playerFormationInfo;
            enemyFormationInfo = staticData.enemyFormationInfo;
        }
    }
}
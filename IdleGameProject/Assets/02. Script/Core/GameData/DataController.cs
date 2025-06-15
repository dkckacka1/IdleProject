using IdleProject.Battle.Spawn;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Core.GameData
{
    [System.Serializable]
    public struct SpawnInfoData
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
        public SpawnInfoData playerSpawnInfo;
        public SpawnInfoData enemySpawnInfo;

        public DataController()
        {
            
        }

        public DataController(TestStaticData staticData)
        {
            playerSpawnInfo = staticData.PlayerSpawnInfo;
            enemySpawnInfo = staticData.EnemySpawnInfo;
        }
    }
}
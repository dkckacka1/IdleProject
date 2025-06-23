using IdleProject.Data;
using IdleProject.Data.Player;

namespace IdleProject.Core.GameData
{
    // 씬 간 데이터 보존 클래스
    [System.Serializable]
    public class DataController
    {
        public PlayerData userData;

        public StageData selectStageData;

        public DataController()
        {
            
        }

        public DataController(PlayerData testPlayerData)
        {
            userData = testPlayerData;
        }
    }
}
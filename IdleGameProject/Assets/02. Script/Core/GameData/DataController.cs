using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Core.GameData
{
    // 씬 간 데이터 보존 클래스
    public class DataController
    {
        public DynamicPlayerData Player;

        public StaticStageData selectStaticStageData;

        public DataController()
        {
            
        }

        public DataController(PlayerData testPlayerData)
        {
            Player = new DynamicPlayerData(testPlayerData);
        }
    }
}
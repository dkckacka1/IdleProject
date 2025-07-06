using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Core.GameData
{
    [CreateAssetMenu(fileName = "ConstData", menuName = "Create/ConstData")]
    // 수정을 하게될 기획 데이터만 정의
    public class ConstData : ScriptableObject
    {
        #region Game

        [FoldoutGroup("Game"), BoxGroup("Game/Loading")]
        public float requiredSceneLoadingTime = 1f;

        [FoldoutGroup("Game"), BoxGroup("Game/Player")]
        public int playerLevelUpExpFactor = 100;
        
        [FoldoutGroup("Game"), BoxGroup("Game/Character")]
        public int characterLevelUpExpFactor = 100;
        
        #endregion
        
        #region Audio
        [FoldoutGroup("Audio")]
        public int minAudioDecibel = 80;
        
        [FoldoutGroup("Audio"), BoxGroup("Audio/BGM")]
        [FilePath(Extensions = "mp3,wav", AbsolutePath = false), SerializeField]
        private string battleSceneBgmName;
        
        [FoldoutGroup("Audio"), BoxGroup("Audio/BGM")]
        [ShowInInspector, ReadOnly]
        public string BattleSceneBgmName => string.IsNullOrEmpty(battleSceneBgmName)
            ? "None"
            : Path.GetFileNameWithoutExtension(battleSceneBgmName);

        [FoldoutGroup("Audio"), BoxGroup("Audio/BGM")]
        [FilePath(Extensions = "mp3,wav", AbsolutePath = false), SerializeField]
        private string lobbySceneBgmName;

        [FoldoutGroup("Audio"), BoxGroup("Audio/BGM")]
        [ShowInInspector, ReadOnly]
        public string LobbySceneBgmName => string.IsNullOrEmpty(lobbySceneBgmName)
            ? "None"
            : Path.GetFileNameWithoutExtension(lobbySceneBgmName);

        #endregion

        #region BattleLogic

        [FoldoutGroup("Battle")]
        public float minBattleSpeed = 1f;
        
        [FoldoutGroup("Battle")]
        public float maxBattleSpeed = 3f;
        
        [FoldoutGroup("Battle"), BoxGroup("Battle/Character")]
        public float defaultGetManaValue = 10f;
        
        [FoldoutGroup("Battle"), BoxGroup("Battle/Character")]
        public float defenseScalingFactor = 100f;
        
        [FoldoutGroup("Battle"), BoxGroup("Battle/Character")]
        public float attackRangeCorrectionValue = 0.1f;

        #endregion

        #region UI

        [FoldoutGroup("UI")]
        public float defaultLongClickInterval = 0.5f;
        [FoldoutGroup("UI"), BoxGroup("UI/Lobby")]
        public float longClickUsePotionInterval = 0.1f;
        [FoldoutGroup("UI"), BoxGroup("UI/Lobby")]
        public float maxLongClickUsePotionInterval = 0.025f;

        #endregion
    }
}
using System;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicCharacterData : DynamicData, ISlotData
    {
        public readonly StaticCharacterData CharacterData;
        public StatValue Stat;

        public string GetIconName => CharacterData.GetIconName;
        
        private DynamicCharacterData(int characterLevel, StaticCharacterData staticCharacterData)
        {
            CharacterData = staticCharacterData;
            SetStat(characterLevel, staticCharacterData);
        }

        private void SetStat(int characterLevel, StaticCharacterData staticCharacterData)
        {
            var levelData = staticCharacterData.levelValue;
            Stat = staticCharacterData.stat;
            Stat.attackDamage += characterLevel > 1 ? levelData.attackDamageValue * (characterLevel - 1) : 0;
            Stat.healthPoint += characterLevel > 1 ? levelData.healthPointValue * (characterLevel - 1) : 0;
        }

        public static DynamicCharacterData GetInstance(PlayerCharacterData characterData)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(characterData.characterName);
            
            return new DynamicCharacterData(characterData.level, staticCharacterData);
        }

        public static DynamicCharacterData GetInstance(PositionInfo info)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(info.characterName);
            
            return new DynamicCharacterData(info.characterLevel, staticCharacterData);
        }
    }
}
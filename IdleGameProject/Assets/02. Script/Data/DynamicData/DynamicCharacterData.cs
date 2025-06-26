using System;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicCharacterData : DynamicData<StaticCharacterData>, ISlotData
    {
        public StatValue Stat;
        public int Level;
        public int Exp;

        public string GetIconName => StaticData.GetIconName;
        public int GetLevelUpExpValue => Level * 100;
        private DynamicCharacterData(int characterLevel, int exp, StaticCharacterData staticCharacterData) : base(staticCharacterData)
        {
            Level = characterLevel;
            Exp = exp;
            SetStat(characterLevel, staticCharacterData);
        }

        public void UpdateCharacter(int characterLevel)
        {
            SetStat(characterLevel, StaticData);
        }

        public void AddExp(int expAmount)
        {
            Exp += expAmount;
            while (Exp > GetLevelUpExpValue)
            {
                Exp -= GetLevelUpExpValue; 
                ++Level;
            }
        }

        private void SetStat(int characterLevel, StaticCharacterData staticCharacterData)
        {
            var levelData = staticCharacterData.levelValue;
            Stat = staticCharacterData.stat;
            Stat.attackDamage += characterLevel > 1 ? levelData.attackDamageValue * (characterLevel - 1) : 0;
            Stat.healthPoint += characterLevel > 1 ? levelData.healthPointValue * (characterLevel - 1) : 0;
        }

        public static int GetLevelExpValue(int level) => level * 100;
        
        public static DynamicCharacterData GetInstance(PlayerCharacterData characterData)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(characterData.characterName);
            
            return new DynamicCharacterData(characterData.level, characterData.exp, staticCharacterData);
        }

        public static DynamicCharacterData GetInstance(PositionInfo info)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(info.characterName);
            
            return new DynamicCharacterData(info.characterLevel, 0, staticCharacterData);
        }
    }
}
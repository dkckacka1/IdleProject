using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        public string characterName;
        public int level = 1;
        public int exp;

        public int equipmentWeaponIndex;
        public int equipmentHelmetIndex;
        public int equipmentArmorIndex;
        public int equipmentGloveIndex;
        public int equipmentBootsIndex;
        public int equipmentAccessoryIndex;
        
        public int GetLevelUpExpValue => level * 100;

        public void AddExp(int expAmount)
        {
            exp += expAmount;
            while (exp > GetLevelUpExpValue)
            {
                exp -= GetLevelUpExpValue; 
                ++level;
            }
        }
        
        public StaticCharacterData GetData => DataManager.Instance.GetData<StaticCharacterData>(characterName);

        public static int GetLevelExpValue(int level) => level * 100;
    }
}

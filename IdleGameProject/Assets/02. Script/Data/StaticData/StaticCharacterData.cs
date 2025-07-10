using IdleProject.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{


    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class StaticCharacterData : StaticData, ISlotData
    {
        public string characterName;

        public string characterAttackName;
        public string characterSkillName;
        public string characterAnimationName;
        
        public StatValue stat;
        [FormerlySerializedAs("levelData")] public LevelValue levelValue;

        public string GetCharacterBannerIconName => $"Icon_{Index}_Banner";
        public string GetCharacterSkillBannerIconName => $"Icon_{Index}_SkillBanner";

        public string GetIconName => GetCharacterBannerIconName;
    }
}



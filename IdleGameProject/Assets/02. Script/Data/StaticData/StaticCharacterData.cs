using System;
using IdleProject.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{


    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class StaticCharacterData : StaticData, ISlotData
    {
        public CharacterAddressValue addressValue;
        public StatValue stat;
        [FormerlySerializedAs("levelData")] public LevelValue levelValue;

        public string GetCharacterBannerIconName => $"Icon_{addressValue.characterName}_Banner";
        public string GetCharacterSkillBannerIconName => $"Icon_{addressValue.characterName}_SkillBanner";

        public string GetIconName => GetCharacterBannerIconName;
    }
}



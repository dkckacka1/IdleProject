using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{
    [Serializable]
    public struct StatValue
    {
        public float healthPoint;
        public float manaPoint;
        public float attackDamage;
        [Range(1f, 10f)]
        public float movementSpeed;
        [Range(2f, 10f)]
        public float attackRange;
        [Range(0.1f, 5f)]
        public float attackCoolTime;
    }

    [Serializable]
    public struct LevelValue
    {
        public float healthPointValue;
        public float attackDamageValue;
    }

    [Serializable]
    public struct CharacterAddressValue
    {
        public string characterName;
        public string characterAnimationName;
        public string attackProjectileAddress;
        public string attackHitEffectAddress;
        public string skillHitEffectAddress;
        public string skillProjectileAddress;
    }

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



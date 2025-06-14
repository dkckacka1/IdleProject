using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data
{
    [Serializable]
    public struct StatData
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
    public struct CharacterAddressValue
    {
        public string characterName;
        public string attackProjectileAddress;
        public string attackHitEffectAddress;
        public string skillHitEffectAddress;
        public string skillProjectileAddress;
    }

    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class CharacterData : Data
    {
        public override string Index => addressValue.characterName;
        
        public CharacterAddressValue addressValue;
        public StatData stat;

    }
}



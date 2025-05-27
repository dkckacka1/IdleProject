using Sirenix.OdinInspector;
using System;
using UnityEngine;

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
    public float attackCooltime;
}

[Serializable]
public struct CharacterAddressValue
{
    public string characterName;
    public string attackProjectileAddress;
    public string attackHitEffectAddress;
    public string skillHitEffectAddress;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterAddressValue addressValue;
    public StatData stat;
}




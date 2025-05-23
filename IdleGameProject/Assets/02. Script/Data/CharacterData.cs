using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StatData
{
    public float healthPoint;
    public float attackDamage;
    [Range(1f, 10f)]
    public float movementSpeed;
    [Range(3f, 10f)]
    public float attackRange;

}

[Serializable]
public struct CharacterAddressValue
{
    public string characterName;
    public string attackProjectileAddress;
    public string attackHitEffectAddress;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterAddressValue addressValue;
    public StatData stat;
}




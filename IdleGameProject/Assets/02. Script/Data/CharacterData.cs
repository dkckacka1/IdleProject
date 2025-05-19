using System;
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
public struct CharacterPrefabValue
{
    public string characterName;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterPrefabValue prefabValue;
    public StatData stat;
}




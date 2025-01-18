using System;
using UnityEngine;

[Serializable]
public struct statData
{
    public float healthPoint;
    public float movementSpeed;
    public float attackDamage;
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
    public statData stat;
}




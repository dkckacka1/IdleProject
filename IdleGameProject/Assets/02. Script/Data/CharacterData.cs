using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public struct StatData
{
    public float healthPoint;
    public float attackDamage;
    [Range(1f, 10f)]
    public float movementSpeed;
    [Range(2f, 10f)]
    public float attackRange;

}

[Serializable]
public struct CharacterAddressValue
{
    public string characterName;
    public string attackProjectileAddress;
    public string attackHitEffectAddress;
}

[Serializable]
public struct CharacterUIOffsetValue
{
    [BoxGroup("FluidHealthBar")] public Vector3 fluidHealthBarOffset;
    [BoxGroup("BattleText")] public Vector3 battleTextOffset;
    [BoxGroup("BattleText")] public float battleTextRadius;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterAddressValue addressValue;
    public StatData stat;
    public CharacterUIOffsetValue uiOffset;
}




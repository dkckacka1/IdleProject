using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core
{
    [System.Serializable]
    public struct PositionInfo
    {
        public string characterName;
        public int characterLevel;
    }
    
    [System.Serializable]
    public struct FormationInfo
    {
        public PositionInfo frontMiddlePositionInfo;
        public PositionInfo frontRightPositionInfo;
        public PositionInfo frontLeftPositionInfo;
        public PositionInfo rearRightPositionInfo;
        public PositionInfo rearLeftPositionInfo;

        public List<string> GetCharacterNameList()
        {
            var result = new List<string>();
            AddString(frontMiddlePositionInfo);
            AddString(frontRightPositionInfo);
            AddString(frontLeftPositionInfo);
            AddString(rearRightPositionInfo);
            AddString(rearLeftPositionInfo);

            return result;
            
            void AddString(PositionInfo info)
            {
                if (string.IsNullOrEmpty(info.characterName) is false)
                {
                    result.Add(info.characterName);
                }
            }
        }
    }
    
    public struct CharacterState
    {
        public bool CanMove;
        public bool CanAttack;
        public bool IsDead;

        public void Initialize()
        {
            CanMove = true;
            CanAttack = true;
            IsDead = false;
        }
    }
    
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
        public float defensePoint;
        public float criticalPercent;
        public float criticalResistance;

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
        public string skillProjectileAddress;
        public string skillHitEffectAddress;
    }

    [Serializable]
    public struct StageInfo
    {
        public int stageIndex;
        public float posX;
        public float posY;
    }

    [Serializable]
    public struct RewardInfo
    {
        public RewardType rewardType;
        public string itemIndex;
        public int count;
    }
}
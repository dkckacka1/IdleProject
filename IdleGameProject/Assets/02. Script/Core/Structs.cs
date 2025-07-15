using System;
using System.Collections.Generic;
using Engine.Util.Extension;
using IdleProject.Core.GameData;
using UnityEngine;

namespace IdleProject.Core
{
    [Serializable]
    public struct PlayerInfo
    {
        public string playerName;
        public int playerLevel;
        public int playerExp;

        public PlayerInfo(string playerName)
        {   
            this.playerName = playerName;
            playerLevel = 1;
            playerExp = 0;
        }

        public void AddExp(int amount)
        {
            playerExp += amount;

            while (playerExp >= playerLevel * DataManager.Instance.ConstData.playerLevelUpExpFactor)
            {
                playerExp -= playerLevel * DataManager.Instance.ConstData.playerLevelUpExpFactor;
                ++playerLevel;
            }
        }

        public int GetMaxExp() => playerLevel * DataManager.Instance.ConstData.playerLevelUpExpFactor;
    }
    
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

        public PositionInfo GetPositionInfo(SpawnPositionType positionType)
        {
            return positionType switch
            {
                SpawnPositionType.FrontMiddle => frontMiddlePositionInfo,
                SpawnPositionType.FrontRight => frontRightPositionInfo,
                SpawnPositionType.FrontLeft => frontLeftPositionInfo,
                SpawnPositionType.RearRight => rearRightPositionInfo,
                SpawnPositionType.RearLeft => rearLeftPositionInfo,
                _ => throw new ArgumentOutOfRangeException(nameof(positionType), positionType, null)
            };
        }
        
        public void SetPositionInfo(SpawnPositionType positionType, PositionInfo newInfo)
        {
            switch (positionType)
            {
                case SpawnPositionType.FrontMiddle: frontMiddlePositionInfo = newInfo; break;
                case SpawnPositionType.FrontRight: frontRightPositionInfo = newInfo; break;
                case SpawnPositionType.FrontLeft: frontLeftPositionInfo = newInfo; break;
                case SpawnPositionType.RearRight: rearRightPositionInfo = newInfo; break;
                case SpawnPositionType.RearLeft: rearLeftPositionInfo = newInfo; break;
                default: throw new ArgumentOutOfRangeException(nameof(positionType), positionType, null);
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

        public float GetStatType(CharacterStatType statType) => statType switch
        {
            CharacterStatType.HealthPoint => healthPoint,
            CharacterStatType.ManaPoint => manaPoint,
            CharacterStatType.MovementSpeed => movementSpeed,
            CharacterStatType.AttackDamage => attackDamage,
            CharacterStatType.AttackRange => attackRange,
            CharacterStatType.AttackCoolTime => attackCoolTime,
            CharacterStatType.DefensePoint => defensePoint,
            CharacterStatType.CriticalPercent => criticalPercent,
            CharacterStatType.CriticalResistance => criticalResistance,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
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
        public string characterAnimationName;
    }

    [Serializable]
    public struct StageInfo
    {
        public int stageIndex;
        public float posX;
        public float posY;
    }
}
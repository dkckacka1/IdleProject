using System;
using System.Collections.Generic;

using Engine.Util.Extension;
using IdleProject.Core;
using IdleProject.Data;
using IdleProject.Data.StaticData;

namespace IdleProject.Battle.Character
{

    public class StatSystem
    {
        public string CharacterName { get; private set; }
        
        private readonly Dictionary<CharacterStatType, BattleCharacterStat> _characterStatDic;

        public bool IsLive => GetStatValue(CharacterStatType.HealthPoint) > 0;
        public bool CanUseSkill => GetStatValue(CharacterStatType.ManaPoint) >= GetStatValue(CharacterStatType.ManaPoint, true);

        public StatSystem()
        {
            _characterStatDic = new Dictionary<CharacterStatType, BattleCharacterStat>();

            EnumExtension.Foreach<CharacterStatType>(type =>
            {
                _characterStatDic.Add(type, new BattleCharacterStat());
            });
        }

        public void SetStatData(string characterName, StatValue statValue)
        {
            CharacterName = characterName;            
            
            _characterStatDic[CharacterStatType.HealthPoint].SetStat(statValue.healthPoint);
            _characterStatDic[CharacterStatType.MovementSpeed].SetStat(statValue.movementSpeed);
            _characterStatDic[CharacterStatType.AttackDamage].SetStat(statValue.attackDamage);
            _characterStatDic[CharacterStatType.AttackRange].SetStat(statValue.attackRange);
            _characterStatDic[CharacterStatType.AttackCoolTime].SetStat(statValue.attackCoolTime);
            _characterStatDic[CharacterStatType.ManaPoint].SetStat(statValue.manaPoint);
        }

        public float GetStatValue(CharacterStatType statType, bool isDefault = false)
        {
            return isDefault ? _characterStatDic[statType].DefaultStatValue : _characterStatDic[statType].Value;
        }

        public void SetStatValue(CharacterStatType statType, float value)
        {
            _characterStatDic[statType].Value = value;
        }

        public void SetStatValue(CharacterStatType setStatType, CharacterStatType getStatType, bool isDefault = false)
        {
            _characterStatDic[setStatType].Value = isDefault ? _characterStatDic[getStatType].DefaultStatValue : _characterStatDic[getStatType].Value;
        }

        public void PublishValueChangedEvent(CharacterStatType statType, Action<float> onValueChanged)
        {
            _characterStatDic[statType].OnValueChanged += onValueChanged;
        }

        public void RemoveValueChangedEvent(CharacterStatType statType, Action<float> onValueChanged)
        {
            _characterStatDic[CharacterStatType.HealthPoint].OnValueChanged -= onValueChanged;
        }
    }
}
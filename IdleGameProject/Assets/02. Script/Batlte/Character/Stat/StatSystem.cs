using System;
using System.Collections.Generic;

using Engine.Util.Extension;

namespace IdleProject.Battle.Character
{
    public enum CharacterStatType
    {
        HealthPoint,
        ManaPoint,
        MovementSpeed,
        AttackDamage,
        AttackRange,
        AttackCooltime,
    }

    public class StatSystem
    {
        StatData defaultStat;

        private Dictionary<CharacterStatType, BattleCharacterStat> characterStatDic;

        public bool isLive => GetStatValue(CharacterStatType.HealthPoint) > 0;
        public bool canUseSkill => GetStatValue(CharacterStatType.ManaPoint) >= GetStatValue(CharacterStatType.ManaPoint, true);

        public StatSystem()
        {
            characterStatDic = new Dictionary<CharacterStatType, BattleCharacterStat>();

            EnumExtension.Foreach<CharacterStatType>(type =>
            {
                characterStatDic.Add(type, new BattleCharacterStat());
            });
        }

        public void SetStatData(StatData statData)
        {
            defaultStat = statData;

            characterStatDic[CharacterStatType.HealthPoint].SetStat(statData.healthPoint);
            characterStatDic[CharacterStatType.MovementSpeed].SetStat(statData.movementSpeed);
            characterStatDic[CharacterStatType.AttackDamage].SetStat(statData.attackDamage);
            characterStatDic[CharacterStatType.AttackRange].SetStat(statData.attackRange);
            characterStatDic[CharacterStatType.AttackCooltime].SetStat(statData.attackCooltime);
            characterStatDic[CharacterStatType.ManaPoint].SetStat(statData.manaPoint);
        }

        public float GetStatValue(CharacterStatType statType, bool isDefault = false)
        {
            return isDefault ? characterStatDic[statType].defaultStatValue : characterStatDic[statType].Value;
        }

        public void SetStatValue(CharacterStatType statType, float value)
        {
            characterStatDic[statType].Value = value;
        }

        public void PublishValueChangedEvent(CharacterStatType statType, Action<float> OnValueChanged)
        {
            characterStatDic[statType].OnValueChanged += OnValueChanged;
        }

        public void RemoveValueChangedEvent(CharacterStatType statType, Action<float> OnValueChanged)
        {
            characterStatDic[CharacterStatType.HealthPoint].OnValueChanged -= OnValueChanged;
        }
    }
}
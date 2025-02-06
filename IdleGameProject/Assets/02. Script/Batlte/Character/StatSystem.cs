using UnityEngine;
using UniRx;

namespace IdleProject.Battle.Character
{
    public enum CharacterStatType
    {
        HealthPoint,
        MovementSpeed,
        AttackDamage,
        AttackRange,
    }

    [System.Serializable]
    public class StatSystem
    {
        StatData defaultStat;

        public ReactiveProperty<float> healthPoint;
        public ReactiveProperty<float> movementSpeed;
        public ReactiveProperty<float> attackDamage;
        public ReactiveProperty<float> attackRange;

        public bool isLive => healthPoint.Value > 0;

        public StatSystem()
        {
            healthPoint = new ReactiveProperty<float>();
            movementSpeed = new ReactiveProperty<float>();
            attackDamage = new ReactiveProperty<float>();
            attackRange = new ReactiveProperty<float>();
        }

        public void SetStatData(StatData statData)
        {
            defaultStat = statData;

            healthPoint.Value = statData.healthPoint;
            movementSpeed.Value = statData.movementSpeed;
            attackDamage.Value = statData.attackDamage;
            attackRange.Value = statData.attackRange;
        }

        public float GetStatValue(CharacterStatType statType, bool isDefault = false)
        {
            switch (statType)
            {
                case CharacterStatType.HealthPoint: return isDefault ? defaultStat.healthPoint : healthPoint.Value;
                case CharacterStatType.MovementSpeed: return isDefault ? defaultStat.movementSpeed : movementSpeed.Value;
                case CharacterStatType.AttackDamage: return isDefault ? defaultStat.attackDamage : attackDamage.Value;
                case CharacterStatType.AttackRange: return isDefault ? defaultStat.attackRange : attackRange.Value;
                default: return 0;
            }
        }

        public void SetStatValue(CharacterStatType statType, float value)
        {
            switch (statType)
            {
                case CharacterStatType.HealthPoint:
                    healthPoint.Value = value;
                    return;
                case CharacterStatType.MovementSpeed:
                    movementSpeed.Value = value;
                    return;
                case CharacterStatType.AttackDamage:
                    attackDamage.Value = value;
                    return;
                case CharacterStatType.AttackRange:
                    attackRange.Value = value;
                    return;
            }
        }
    }
}
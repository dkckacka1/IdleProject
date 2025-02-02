using UnityEngine;
using UniRx;

namespace IdleProject.Battle.Character
{
    [System.Serializable]
    public class StatSystem
    {
        public ReactiveProperty<float> healthPoint;
        public ReactiveProperty<float> movementSpeed;
        public ReactiveProperty<float> attackDamage;
        public ReactiveProperty<float> attackRange;

        public StatSystem()
        {
            healthPoint = new ReactiveProperty<float>();
            movementSpeed = new ReactiveProperty<float>();
            attackDamage = new ReactiveProperty<float>();
            attackRange = new ReactiveProperty<float>();
        }

        public void SetStatData(StatData statData)
        {
            healthPoint.Value = statData.healthPoint;
            movementSpeed.Value = statData.movementSpeed;
            attackDamage.Value = statData.attackDamage;
            attackRange.Value = statData.attackRange;
        }
    }
}
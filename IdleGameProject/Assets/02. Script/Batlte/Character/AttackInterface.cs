using UnityEngine;

namespace IdleProject.Battle.Character
{
    public interface ITargetedAble
    {
        public Transform GetTransform { get; }
    }

    public interface ITakeDamagedAble : ITargetedAble
    {
        public Transform HitEffectTransform { get; }
        public bool CanTakeDamage { get; }
        public void TakeDamage(float attackDamage);
    }
}
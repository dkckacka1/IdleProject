using UnityEngine;

namespace IdleProject.Battle.Character
{
    public interface ITargetedAble
    {
    }

    public interface ITakeDamagedAble : ITargetedAble
    {
        public void TakeDamage(float attackDamage);
    }
}
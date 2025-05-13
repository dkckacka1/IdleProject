using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public bool canAttack;

        public Func<ITakeDamagedAble> GetTargetCharacter;


        protected virtual void SetBattleAnimEvent()
        {
            animController.AnimEventHandler.AttackEvent += HitTarget;
        }

        public virtual void Attack()
        {
            animController.SetAttack();
        }

        public void HitTarget()
        {
            var targetCharacter = GetTargetCharacter?.Invoke();
            if (targetCharacter is not null)
            {
                Hit(targetCharacter);
            }
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage)
        {
            iTakeDamage.TakeDamage(statSystem.GetStatValue(CharacterStatType.AttackDamage));
        }

        public virtual void TakeDamage(float takeDamage)
        {
            statSystem.SetStatValue(CharacterStatType.HealthPoint, statSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);

            if (statSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            animController.SetDeath();
        }
    }
}
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public bool canAttack;

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
            ITakeDamagedAble takeDamageTarget = ai.target;

            if (takeDamageTarget is not null)
            {
                Hit(takeDamageTarget);
            }
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage)
        {
            iTakeDamage.TakeDamage(statSystem.GetStatValue(CharacterStatType.AttackDamage));
        }

        public virtual void TakeDamage(float takeDamage)
        {
            statSystem.SetStatValue(CharacterStatType.HealthPoint, statSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);
        }
    }
}
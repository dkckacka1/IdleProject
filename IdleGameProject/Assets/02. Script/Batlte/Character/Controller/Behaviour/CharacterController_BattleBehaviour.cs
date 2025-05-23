using IdleProject.Battle.Effect;
using IdleProject.Core.ObjectPool;
using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public Func<ITakeDamagedAble> GetTargetCharacter;

        public bool CanTakeDamage => !state.isDead; 

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
            if (iTakeDamage.CanTakeDamage)
            {
                if (GetAttackHitEffect is not null)
                {
                    var attackHitEffect =GetAttackHitEffect?.Invoke();
                    attackHitEffect.transform.position = iTakeDamage.GetTransform.position;
                }
                iTakeDamage.TakeDamage(statSystem.GetStatValue(CharacterStatType.AttackDamage));
            }
        }

        public virtual void TakeDamage(float takeDamage)
        {
            statSystem.SetStatValue(CharacterStatType.HealthPoint, statSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);

            if (statSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
            {
                Death();
                BattleManager.Instance.DeathCharacter(this, characterAI.aiType);
            }
        }

        private void Death()
        {
            state.isDead = true;
            animController.SetDeath();
        }
    }
}
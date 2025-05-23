using IdleProject.Battle.Effect;
using IdleProject.Core.ObjectPool;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        [BoxGroup("ITakeDamage"), SerializeField] private Transform hitEffectPosition;

        public Func<ITakeDamagedAble> GetTargetCharacter;
        public bool CanTakeDamage => !state.isDead;

        public Transform HitEffectPosition => hitEffectPosition;

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
                if (GetAttackHitEffect is not null && iTakeDamage.HitEffectPosition is not null)
                {
                    var attackHitEffect = GetAttackHitEffect?.Invoke();
                    attackHitEffect.transform.position = iTakeDamage.HitEffectPosition.position;
                }

                var attackDamage = statSystem.GetStatValue(CharacterStatType.AttackDamage);

                iTakeDamage.TakeDamage(attackDamage);
            }
        }

        public virtual void TakeDamage(float takeDamage)
        {
            characterUI.ShowBattleText(takeDamage.ToString());
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
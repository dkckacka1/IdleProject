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

        [SerializeField] private Transform projectileCreatePosition;

        public Func<ITakeDamagedAble> GetTargetCharacter;
        public bool CanTakeDamage => !state.isDead;

        public Transform HitEffectTransform => hitEffectPosition;

        protected virtual void SetBattleAnimEvent()
        {
            animController.AnimEventHandler.AttackEvent += HitTarget;
        }

        public virtual void Attack()
        {
            transform.LookAt(GetTargetCharacter?.Invoke().GetTransform);
            animController.SetAttack();
        }


        public void HitTarget()
        {
            var targetCharacter = GetTargetCharacter?.Invoke();

            if (GetAttackProjectile is not null)
            {
                var projectile = GetAttackProjectile.Invoke();
                projectile.transform.position = projectileCreatePosition.position;
                projectile.target = targetCharacter;
                projectile.hitEvent.AddListener(Hit);
            }
            else
            {
                if (targetCharacter is not null)
                {
                    Hit(targetCharacter);
                }
            }
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage)
        {
            if (iTakeDamage.CanTakeDamage)
            {
                if (GetAttackHitEffect is not null && iTakeDamage.HitEffectTransform is not null)
                {
                    var attackHitEffect = GetAttackHitEffect?.Invoke();
                    attackHitEffect.transform.position = iTakeDamage.HitEffectTransform.position;
                }

                var attackDamage = statSystem.GetStatValue(CharacterStatType.AttackDamage);

                iTakeDamage.TakeDamage(attackDamage);
            }
        }

        public virtual void TakeDamage(float takeDamage)
        {
            GetCharacterUI?.Invoke().ShowBattleText(takeDamage.ToString());
            statSystem.SetStatValue(CharacterStatType.HealthPoint, statSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);

            if (statSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            BattleManager.Instance.DeathCharacter(this);
            state.isDead = true;
            animController.SetDeath();
        }
    }
}
using Cysharp.Threading.Tasks;
using IdleProject.Battle.Effect;
using IdleProject.Core.ObjectPool;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public Func<ITakeDamagedAble> GetTargetCharacter;

        private const float DefaultGetManaPoint = 10;
        [ShowInInspector] private bool isNowAttack;
        [ShowInInspector] private bool isNowSkill;

        public bool CanTakeDamage => !state.isDead;
        public Vector3 HitEffectOffset => offset.HitEffecOffset;

        protected virtual void SetBattleAnimEvent()
        {
            animController.AnimEventHandler.AttackStartEvent += OnAttackStart;
            animController.AnimEventHandler.AttackHitEvent += OnAttackHit;
            animController.AnimEventHandler.AttackEndEvent += OnAttackEnd;
            animController.AnimEventHandler.SkillStartEvent += OnSkillStart;
            animController.AnimEventHandler.SkillEndEvent += OnSkillEnd;
        }

        private void OnAttackStart()
        {
            isNowAttack = true;
        }


        public virtual void Attack()
        {
            transform.LookAt(GetTargetCharacter?.Invoke().GetTransform);
            if (isNowAttack is false && isNowSkill is false)
            {
                animController.SetAttack();
            }
        }

        public void OnAttackHit()
        {
            var targetCharacter = GetTargetCharacter?.Invoke();

            if (targetCharacter is not null)
            {
                if (GetAttackProjectile is not null)
                {
                    var projectile = GetAttackProjectile.Invoke();
                    projectile.transform.position = offset.CreateProjectileOffset;
                    projectile.target = targetCharacter;
                    projectile.hitEvent.AddListener(Hit);
                }
                else
                {
                    Hit(targetCharacter);
                }
            }
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage)
        {
            if (iTakeDamage.CanTakeDamage)
            {
                if (GetAttackHitEffect is not null)
                {
                    var attackHitEffect = GetAttackHitEffect?.Invoke();
                    attackHitEffect.transform.position = iTakeDamage.HitEffectOffset;
                }

                var attackDamage = statSystem.GetStatValue(CharacterStatType.AttackDamage);

                iTakeDamage.TakeDamage(attackDamage);
                GetMana();
            }
        }

        public virtual void TakeDamage(float takeDamage)
        {
            characterUI.ShowBattleText(takeDamage.ToString());
            statSystem.SetStatValue(CharacterStatType.HealthPoint, statSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);

            if (statSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
            {
                Death();
            }
        }


        private void OnAttackEnd()
        {
            isNowAttack = false;
            StartAttackCooltime().Forget();
        }

        private async UniTaskVoid StartAttackCooltime()
        {
            state.canAttack = false;
            await UniTask.WaitForSeconds(statSystem.GetStatValue(CharacterStatType.AttackCooltime));
            state.canAttack = true;
        }



        #region 스킬 관련
        public virtual void Skill()
        {
            if (isNowAttack is false && isNowSkill is false)
            {
                animController.SetSkill();
            }

        }

        private void OnSkillStart()
        {
            isNowSkill = true;
            statSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
        }

        private void OnSkillEnd()
        {
            isNowSkill = false;
            StartAttackCooltime().Forget();
        }

        private void GetMana()
        {
            statSystem.SetStatValue(CharacterStatType.ManaPoint, statSystem.GetStatValue(CharacterStatType.ManaPoint) + DefaultGetManaPoint);
        }
        #endregion


        private void Death()
        {
            BattleManager.Instance.DeathCharacter(this);
            state.isDead = true;
            animController.SetDeath();
        }
    }
}
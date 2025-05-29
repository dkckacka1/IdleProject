using Cysharp.Threading.Tasks;
using IdleProject.Battle.Effect;
using IdleProject.Core.ObjectPool;
using Sirenix.OdinInspector;
using System;
using Engine.Core.Time;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public Func<ITakeDamagedAble> GetTargetCharacter;

        private const float DefaultGetManaPoint = 10;
        [ShowInInspector] private bool isNowAttack;
        [ShowInInspector] private bool isNowSkill;

        public bool CanTakeDamage => !State.isDead;
        public bool HasSkill => _skill is not null;
        public Vector3 HitEffectOffset => offset.HitEffecOffset;

        protected virtual void SetBattleAnimEvent()
        {
            AnimController.AnimEventHandler.AttackStartEvent += OnAttackStart;
            AnimController.AnimEventHandler.AttackHitEvent += OnAttackHit;
            AnimController.AnimEventHandler.AttackEndEvent += OnAttackEnd;
            AnimController.AnimEventHandler.SkillStartEvent += OnSkillStart;
            AnimController.AnimEventHandler.SkillEndEvent += OnSkillEnd;
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
                AnimController.SetAttack();
            }
        }

        public void OnAttackHit()
        {
            var targetCharacter = GetTargetCharacter?.Invoke();

            if (targetCharacter is not null)
            {
                var attackDamage = StatSystem.GetStatValue(CharacterStatType.AttackDamage);
                if (GetAttackProjectile is not null)
                {
                    var projectile = GetAttackProjectile.Invoke();
                    projectile.transform.position = offset.CreateProjectileOffset;
                    projectile.target = targetCharacter;
                    projectile.hitEvent.AddListener(target =>
                    {
                        Hit(target, attackDamage);
                    });
                }
                else
                {
                    Hit(targetCharacter, attackDamage);
                }
            }

            void Hit(ITakeDamagedAble target, float attackDamage)
            {
                this.Hit(target, attackDamage);
                
                var attackHitEffect = GetAttackHitEffect?.Invoke();
                if (attackHitEffect != null) 
                    attackHitEffect.transform.position = target.HitEffectOffset;
                
                GetMana();
            }
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage, float attackDamage)
        {
            if (iTakeDamage.CanTakeDamage)
            {
                iTakeDamage.TakeDamage(attackDamage);
            }
        }

        public virtual void TakeDamage(float takeDamage)
        {
            characterUI.ShowBattleText(takeDamage.ToString());
            StatSystem.SetStatValue(CharacterStatType.HealthPoint, StatSystem.GetStatValue(CharacterStatType.HealthPoint) - takeDamage);

            if (StatSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
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
            State.canAttack = false;
            await BattleManager.GetBattleTimer(StatSystem.GetStatValue(CharacterStatType.AttackCooltime));
            State.canAttack = true;
        }

        #region 스킬 관련
        public virtual void Skill()
        {
            if (isNowAttack is false && isNowSkill is false)
            {
                AnimController.SetSkill();
            }
        }

        private void OnSkillStart()
        {
            isNowSkill = true;
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
            BattleManager.Instance.AddSkillQueue(this);
        }

        private void OnSkillEnd()
        {
            isNowSkill = false;
            StartAttackCooltime().Forget();

            if (GetSkillProjectile is null)
            {
                BattleManager.Instance.ExitSkill();
            }
        }

        private void GetMana()
        {
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, StatSystem.GetStatValue(CharacterStatType.ManaPoint) + DefaultGetManaPoint);
        }
        #endregion


        private void Death()
        {
            BattleManager.Instance.DeathCharacter(this);
            characterUI.OnCharacterDeath();
            State.isDead = true;
            AnimController.SetDeath();
            collider.enabled = false;
        }
    }
}
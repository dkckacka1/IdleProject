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

        private const float DEFAULT_GET_MANA_POINT = 10;
        private bool _isNowAttack;
        private bool _isNowSkill;

        public bool CanTakeDamage => !State.IsDead;
        public bool HasSkill => _skill is not null;
        public Vector3 HitEffectOffset => offset.HitEffectOffset;

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
            _isNowAttack = true;
        }


        public virtual void Attack()
        {
            transform.LookAt(GetTargetCharacter?.Invoke().GetTransform);
            if (_isNowAttack is false && _isNowSkill is false)
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
                    projectile.Target = targetCharacter;
                    projectile.hitEvent.AddListener(target =>
                    {
                        HitTarget(target, attackDamage);
                    });
                }
                else
                {
                    HitTarget(targetCharacter, attackDamage);
                }
            }

            return;

            void HitTarget(ITakeDamagedAble target, float attackDamage)
            {
                Hit(target, attackDamage);
                
                var attackHitEffect = GetAttackHitEffect?.Invoke();
                if (attackHitEffect) 
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
            _isNowAttack = false;
            StartAttackCooltime().Forget();
        }

        private async UniTaskVoid StartAttackCooltime()
        {
            State.CanAttack = false;
            await BattleManager.GetBattleTimer(StatSystem.GetStatValue(CharacterStatType.AttackCoolTime));
            State.CanAttack = true;
        }

        #region 스킬 관련
        public virtual void Skill()
        {
            if (_isNowAttack is false && _isNowSkill is false)
            {
                AnimController.SetSkill();
            }
        }

        private void OnSkillStart()
        {
            _isNowSkill = true;
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
            BattleManager.Instance.AddSkillQueue(this);
        }

        private void OnSkillEnd()
        {
            _isNowSkill = false;
            StartAttackCooltime().Forget();

            if (GetSkillProjectile is null)
            {
                BattleManager.Instance.ExitSkill();
            }
        }

        private void GetMana()
        {
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, StatSystem.GetStatValue(CharacterStatType.ManaPoint) + DEFAULT_GET_MANA_POINT);
        }
        #endregion


        private void Death()
        {
            BattleManager.Instance.DeathCharacter(this);
            characterUI.OnCharacterDeath();
            State.IsDead = true;
            AnimController.SetDeath();
            GetComponent<Collider>().enabled = false;
        }
    }
}
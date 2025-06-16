using Cysharp.Threading.Tasks;
using IdleProject.Battle.Effect;
using IdleProject.Core.ObjectPool;
using Sirenix.OdinInspector;
using System;
using Engine.Core.Time;
using IdleProject.Battle.AI;
using IdleProject.Battle.UI;
using IdleProject.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeDamagedAble
    {
        public Func<ITakeDamagedAble> GetTargetCharacter;
        public bool isNowSkill;
        public bool isNowAttack;

        private const float DEFAULT_GET_MANA_POINT = 10;

        public Transform GetTransform => transform;
        public CharacterAIType GetAiType => characterAI.aiType;

        public bool CanTakeDamage => !State.IsDead;
        public bool HasSkill => CharacterSkill is not null;
        public Vector3 HitEffectOffset => offset.GetHitEffectPosition;

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
                    projectile.transform.position = offset.GetProjectilePosition;
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
            isNowAttack = false;
            StartAttackCooltime().Forget();
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
            GameManager.GetCurrentSceneManager<BattleManager>().AddSkillQueue(this);
            
            (characterUI as PlayerCharacterUIController)?.StartSkill();
        }

        private void OnSkillEnd()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().RemoveSkillObject(this);
            
            (characterUI as PlayerCharacterUIController)?.EndSkill();
        }
        
        public async UniTaskVoid StartAttackCooltime()
        {
            State.CanAttack = false;
            await BattleManager.GetBattleTimer(StatSystem.GetStatValue(CharacterStatType.AttackCoolTime));
            State.CanAttack = true;
        }

        private void GetMana()
        {
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, StatSystem.GetStatValue(CharacterStatType.ManaPoint) + DEFAULT_GET_MANA_POINT);
        }
        #endregion


        private void Death()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().DeathCharacter(this);
            characterUI.OnCharacterDeath();
            State.IsDead = true;
            AnimController.SetDeath();
            GetComponent<Collider>().enabled = false;
        }
    }
}
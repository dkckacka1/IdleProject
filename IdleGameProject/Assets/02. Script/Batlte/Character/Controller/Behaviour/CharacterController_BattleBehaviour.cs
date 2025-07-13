using System;
using Cysharp.Threading.Tasks;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : ITakeCriticalAble
    {
        public Func<CharacterController> GetTargetCharacter;
        public bool isNowSkill;
        public bool isNowAttack;

        public Transform GetTransform => transform;
        public CharacterAIType GetAiType => characterAI.aiType;
        public bool CanTakeDamage => !State.IsDead;
        public bool HasSkill => CharacterSkill is not null;
        public Vector3 HitEffectOffset => offset.GetOffsetTransform(CharacterOffsetType.HitOffset).position;
        public float GetCriticalResist => StatSystem.GetStatValue(CharacterStatType.CriticalResistance);

        protected virtual void SetBattleAnimEvent()
        {
            AnimController.AnimEventHandler.AttackStartEvent += OnAttackStart;
            AnimController.AnimEventHandler.AttackActionEvent += OnAttackAction;
            AnimController.AnimEventHandler.AttackEndEvent += OnAttackEnd;
            AnimController.AnimEventHandler.SkillStartEvent += OnSkillStart;
            AnimController.AnimEventHandler.SkillEndEvent += OnSkillEnd;
            
            if (CharacterSkill is not null)
            {
                AnimController.AnimEventHandler.SkillActionEvent += CharacterSkill.ExecuteSkill;
            }
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

        public void OnAttackAction(int attackNumber)
        {
            CharacterAttack.ExecuteSkill();
            GetMana();
        }

        public virtual void Hit(ITakeDamagedAble iTakeDamage, float attackDamage, bool canCritical)
        {
            if (iTakeDamage.CanTakeDamage)
            {
                if (canCritical && iTakeDamage is ITakeCriticalAble takeCritical &&
                    CheckCritical(StatSystem.GetStatValue(CharacterStatType.CriticalPercent), takeCritical))
                {
                    attackDamage = attackDamage * DataManager.Instance.ConstData.criticalAttackFactor;
                }

                iTakeDamage.TakeDamage(attackDamage);
            }
        }

        private bool CheckCritical(float critRate, ITakeCriticalAble takeCriticalAble)
        {
            var finalCritChance = Mathf.Clamp01(critRate - takeCriticalAble.GetCriticalResist);
            var isCrit = UnityEngine.Random.value < finalCritChance;
            return isCrit;
        }

        public virtual void TakeDamage(float takeDamage)
        {
            var damage = CalculateTakeDamage(takeDamage);

            characterUI.ShowBattleText(damage.ToString("0"));
            StatSystem.SetStatValue(CharacterStatType.HealthPoint,
                StatSystem.GetStatValue(CharacterStatType.HealthPoint) - damage);

            if (StatSystem.GetStatValue(CharacterStatType.HealthPoint) <= 0)
            {
                Death();
            }
        }

        private float CalculateTakeDamage(float takeDamage)
        {
            var defenseScaleFactor = DataManager.Instance.ConstData.defenseScalingFactor;

            var damage = takeDamage * (defenseScaleFactor /
                                       (defenseScaleFactor + StatSystem.GetStatValue(CharacterStatType.DefensePoint)));
            return damage;
        }

        private void OnAttackEnd()
        {
            isNowAttack = false;
            StartAttackCoolTime().Forget();
        }

        #region 스킬 관련

        public virtual void Skill()
        {
            if (isNowAttack is false && isNowSkill is false)
            {
                AnimController.SetSkill();
                GetBattleManager.AddSkillQueue(this);
                isNowSkill = true;
            }
        }

        private void OnSkillStart()
        {
        }

        private void OnSkillEnd()
        {
            GetBattleManager.RemoveSkillObject(this);
        }

        public void AccessSkill()
        {
            isNowSkill = true;
            (characterUI as PlayerCharacterUIController)?.StartSkill();
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
        }

        public void ExitSkill()
        {
            isNowSkill = false;
            isNowAttack = false;
            (characterUI as PlayerCharacterUIController)?.EndSkill();
            StartAttackCoolTime().Forget();
        }

        public async UniTaskVoid StartAttackCoolTime()
        {
            State.CanAttack = false;
            await GetBattleManager.GetBattleTimer(StatSystem.GetStatValue(CharacterStatType.AttackCoolTime));
            State.CanAttack = true;
        }

        private void GetMana()
        {
            StatSystem.SetStatValue(CharacterStatType.ManaPoint,
                StatSystem.GetStatValue(CharacterStatType.ManaPoint) +
                DataManager.Instance.ConstData.defaultGetManaValue);
        }

        #endregion

        private void Death()
        {
            GetBattleManager.DeathCharacter();
            characterUI.OnCharacterDeath();
            State.IsDead = true;
            AnimController.SetDeath();
            GetComponent<Collider>().enabled = false;
            Agent.enabled = false;
        }

        public static bool IsTargetInsideAttackRange(CharacterController mine, ITargetedAble targetCharacter)
        {
            return Vector3.Distance(mine, targetCharacter.GetTransform.position) <
                   mine.StatSystem.GetStatValue(CharacterStatType.AttackRange);
        }
    }
}
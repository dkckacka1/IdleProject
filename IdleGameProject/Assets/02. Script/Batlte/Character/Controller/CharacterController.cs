using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Effect;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Battle.Projectile;
using System;
using UnityEngine;
using UnityEngine.AI;
using IdleProject.Battle.Character.Skill;
using Engine.Core;
using Zenject;
using IPoolable = IdleProject.Core.ObjectPool.IPoolable;

namespace IdleProject.Battle.Character
{
    public struct CharacterState
    {
        public bool CanMove;
        public bool CanAttack;
        public bool IsDead;

        public void Initialize()
        {
            CanMove = true;
            CanAttack = true;
            IsDead = false;
        }
    }

    [System.Serializable]
    public partial class CharacterController : MonoBehaviour
    {
        [Inject] private BattleManager _battleManager;
        [Inject] private BattleResourceLoader _battleResourceLoader; 
        
        public StatSystem StatSystem;
        public CharacterState State;
        public CharacterOffset offset;

        [HideInInspector] public CharacterUIController characterUI;
        [HideInInspector] public CharacterAIController characterAI;
        [HideInInspector] public AnimationController AnimController;

        private CharacterSkill _skill;
        protected NavMeshAgent Agent;

        public Func<BattleEffect> GetAttackHitEffect;
        public Func<BattleEffect> GetSkillHitEffect;
        public Func<BattleProjectile> GetAttackProjectile;
        public Func<BattleProjectile> GetSkillProjectile;
        
        public Transform GetTransform => transform;
        
        #region 초기화 부문
        
        [Inject]
        public virtual void Initialized(CharacterData data, CharacterAIType aiType, AnimationController animController)
        {
            InitDependencies();
            
            SetSkill(data);
            SetStatModifyEvent();
            SetAnimationEvent();
            
            SetCharacterData(data.stat);
            
            InitPoolableObject(data);
            
            State.Initialize();
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
            return;

            void InitDependencies()
            {
                offset = GetComponent<CharacterOffset>();
                Agent = GetComponent<NavMeshAgent>();
                
                AnimController = animController;
                StatSystem = new StatSystem();
            }
        }

        private void SetSkill(CharacterData data)
        {
            var skillName = $"{typeof(CharacterSkill).FullName}{data.addressValue.characterName}, {typeof(CharacterSkill).Assembly}";

            if (Type.GetType(skillName) is not null)
            {
                _skill = ReflectionController.CreateInstance<CharacterSkill>(skillName);
                _skill.Controller = this;
                _skill.SetAnimationEvent(AnimController.AnimEventHandler);
            }
        }
        private void SetAnimationEvent()
        {
            AnimController.OnTimeFactorChange(BattleManager.GetCurrentBattleSpeed);
            
            BattleManager.GetChangeBattleSpeedEvent.AddListener(AnimController.OnTimeFactorChange);
            _battleManager.GameStateEventBus.PublishEvent(AnimController);
            SetBattleAnimEvent();
        }
        protected virtual void SetCharacterData(StatData stat)
        {
            StatSystem.SetStatData(stat);
        }

        private async UniTask InitPoolableObject(CharacterData data)
        {
            GetAttackHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.attackHitEffectAddress);
            GetSkillHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.skillHitEffectAddress);
            GetAttackProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.attackProjectileAddress);
            GetSkillProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.skillProjectileAddress);
        }

        private async UniTask<Func<T>> CreatePool<T>(PoolableType poolableType, string address) where T : IPoolable
        {
            if (string.IsNullOrEmpty(address) is true) return null;

            await _battleResourceLoader.CreatePool(poolableType, address);
            return () => _battleResourceLoader.GetPoolableObject<T>(poolableType, address);
        }
        #endregion

        public void Win()
        {
            AnimController.SetWin();
        }

        public void Idle()
        {
            AnimController.SetIdle();
        }

        public static implicit operator Vector3(CharacterController controller) => controller.transform.position;

        public static implicit operator Transform(CharacterController controller) => controller.transform;

    }
}
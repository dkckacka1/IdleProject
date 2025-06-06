using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Effect;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using IdleProject.Battle.Projectile;
using System;
using UnityEngine;
using UnityEngine.AI;
using IdleProject.Battle.Character.Skill;
using Engine.Core;

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
        public StatSystem StatSystem;
        public CharacterState State;
        public CharacterOffset offset;

        [HideInInspector] public CharacterUIController characterUI;
        [HideInInspector] public CharacterAIController characterAI;
        [HideInInspector] public AnimationController AnimController;

        private CharacterSkill _skill;

        protected NavMeshAgent Agent;
        private Collider _collider;

        public Func<BattleEffect> GetAttackHitEffect;
        public Func<BattleEffect> GetSkillHitEffect;
        public Func<BattleProjectile> GetAttackProjectile;
        public Func<BattleProjectile> GetSkillProjectile;

        public Transform GetTransform => transform;

        private void Awake()
        {
            offset = GetComponent<CharacterOffset>();
            
            Agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();

            StatSystem = new StatSystem();

            AnimController = new AnimationController(GetComponentInChildren<Animator>(), GetComponentInChildren<AnimationEventHandler>());
        }

        #region 초기화 부문
        public virtual void Initialized(CharacterData data, CharacterAIType aiType)
        {
            SetSkill(data);
            SetStatModifyEvent();
            SetAnimationEvent();

            SetCharacterData(data.stat);

            InitUIController(data, aiType);
            InitAIController(aiType);
            InitPoolableObject(data);

            State.Initialize();
            StatSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
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
            BattleManager.Instance.GameStateEventBus.PublishEvent(AnimController);
            SetBattleAnimEvent();
        }
        protected virtual void SetCharacterData(StatData stat)
        {
            StatSystem.SetStatData(stat);
        }

        private async UniTask InitUIController(CharacterData data, CharacterAIType aiType)
        {
            CharacterUIController uiController = null;

            switch (aiType)
            {
                case CharacterAIType.Player:
                    uiController = gameObject.AddComponent<PlayerCharacterUIController>();
                    break;
                case CharacterAIType.Enemy:
                    uiController = gameObject.AddComponent<CharacterUIController>();
                    break;
            }

            uiController.Initialized(data, StatSystem);
            BattleManager.Instance.BattleObjectEventDic[BattleObjectType.UI].AddListener(uiController.OnBattleUIEvent);

            characterUI = uiController;
        }

        private async UniTask InitAIController(CharacterAIType aiType)
        {
            var aiController = gameObject.AddComponent<CharacterAIController>();
            aiController.aiType = aiType;

            BattleManager.Instance.BattleObjectEventDic[BattleObjectType.Character].AddListener(aiController.OnBatteEvent);

            characterAI = aiController;
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

            await ResourcesLoader.CreatePool(poolableType, address);
            return () => ResourcesLoader.GetPoolableObject<T>(poolableType, address);
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
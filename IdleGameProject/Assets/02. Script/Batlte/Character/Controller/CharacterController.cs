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
    public partial struct CharacterState
    {
        public bool canMove;
        public bool canAttack;
        public bool isDead;

        public void Initialize()
        {
            canMove = true;
            canAttack = true;
            isDead = false;
        }
    }

    [System.Serializable]
    public partial class CharacterController : MonoBehaviour
    {
        public StatSystem statSystem;
        public CharacterState state;

        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationController animController;

        private CharacterOffset offset;

        public Transform GetTransform => transform;

        public CharacterUIController characterUI;
        public CharacterAIController characterAI;

        public Func<BattleEffect> GetAttackHitEffect;
        public Func<BattleEffect> GetSkillHitEffect;
        public Func<BattleProjectile> GetAttackProjectile;

        private CharacterSkill skill;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            offset = GetComponent<CharacterOffset>();

            statSystem = new StatSystem();

            animController = new AnimationController(animator, GetComponentInChildren<AnimationEventHandler>());
        }

        #region 초기화 부문
        public virtual void Initialized(CharacterData data, CharacterAIType aiType)
        {
            string skillName = $"{typeof(CharacterSkill).FullName}{data.addressValue.characterName}, {typeof(CharacterSkill).Assembly}";
            skill = ReflectionController.CreateInstance<CharacterSkill>(skillName);

            SetStatModifedEvent();
            SetAnimationEvent();

            SetCharacterData(data.stat);

            InitUIController(data, aiType);
            InitAIController(aiType);
            InitPoolableObject(data);

            state.Initialize();
            statSystem.SetStatValue(CharacterStatType.ManaPoint, 0);
        }

        private void SetAnimationEvent()
        {
            SetBattleAnimEvent();
        }
        protected virtual void SetCharacterData(StatData stat)
        {
            statSystem.SetStatData(stat);
        }

        private async UniTask InitUIController(CharacterData data, CharacterAIType aiType)
        {
            CharacterUIController uiController = null;

            switch (aiType)
            {
                case CharacterAIType.Playerable:
                    uiController = gameObject.AddComponent<PlayerCharacterUIController>();
                    break;
                case CharacterAIType.Enemy:
                    uiController = gameObject.AddComponent<CharacterUIController>();
                    break;
            }

            uiController.Initialized(data, statSystem);
            BattleManager.Instance.battleUIEvent.AddListener(uiController.OnBattleUIEvent);

            characterUI = uiController;
        }

        private async UniTask InitAIController(CharacterAIType aiType)
        {
            var aiController = gameObject.AddComponent<CharacterAIController>();
            aiController.aiType = aiType;

            BattleManager.Instance.battleEvent.AddListener(aiController.OnBatteEvent);
            BattleManager.Instance.battleStateEventBus.PublishEvent(BattleStateType.Win, aiController.OnWinEvent);
            BattleManager.Instance.battleStateEventBus.PublishEvent(BattleStateType.Defeat, aiController.OnDefeatEvent);

            characterAI = aiController;
        }

        private async UniTask InitPoolableObject(CharacterData data)
        {
            GetAttackHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.attackHitEffectAddress);
            GetSkillHitEffect = await CreatePool<BattleEffect>(PoolableType.Effect, data.addressValue.skillHitEffectAddress);
            GetAttackProjectile = await CreatePool<BattleProjectile>(PoolableType.Projectile, data.addressValue.attackProjectileAddress);
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
            animController.SetWin();
        }

        public void Idle()
        {
            animController.SetIdle();
        }

        public static implicit operator Vector3(CharacterController controller) => controller.transform.position;

        public static implicit operator Transform(CharacterController controller) => controller.transform;

    }
}
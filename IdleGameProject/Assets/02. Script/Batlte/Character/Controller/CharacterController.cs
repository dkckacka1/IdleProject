using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.Effect;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using System;
using UnityEngine;
using UnityEngine.AI;

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

        public Transform GetTransform => transform;

        [HideInInspector] public CharacterUIController characterUI;
        [HideInInspector] public CharacterAIController characterAI;

        private bool isUIInit = false;
        private bool isPoolObject = false;

        private Func<BattleEffect> GetAttackHitEffect;

        private bool IsInitComplete => isUIInit && isPoolObject;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            statSystem = new StatSystem();

            animController = new AnimationController(animator, GetComponentInChildren<AnimationEventHandler>());

            characterUI = GetComponent<CharacterUIController>();
            characterAI = GetComponent<CharacterAIController>();
        }

        #region 초기화 부문
        public async virtual UniTask Initialized(CharacterData data)
        {
            SetStatModifedEvent();
            SetAnimationEvent();

            SetCharacterData(data.stat);
            SetCharacterAI();
            SetCharacterUI().Forget();
            CreatePoolableObject(data.addressValue).Forget();

            state.Initialize();

            await UniTask.WaitUntil(() => IsInitComplete);
        }

        private void SetAnimationEvent()
        {
            SetBattleAnimEvent();
        }

        protected virtual void SetCharacterData(StatData stat)
        {
            statSystem.SetStatData(stat);
        }

        private void SetCharacterAI()
        {
            BattleManager.Instance.battleEvent.AddListener(characterAI.OnBatteEvent);
            BattleManager.Instance.battleStateEventBus.PublishEvent(BattleStateType.Win, characterAI.OnWinEvent);
            BattleManager.Instance.battleStateEventBus.PublishEvent(BattleStateType.Defeat, characterAI.OnDefeatEvent);
        }

        private async UniTaskVoid SetCharacterUI()
        {
            await characterUI.SpawnCharacterUI();
            characterUI.SetCharacterUI(statSystem);
            BattleManager.Instance.battleUIEvent.AddListener(characterUI.BattleAction);
            isUIInit = true;
        }

        private async UniTaskVoid CreatePoolableObject(CharacterAddressValue addressValues)
        {
            if (string.IsNullOrEmpty(addressValues.attackHitEffectAddress) is false)
            {
                await  ResourcesLoader.CreatePool(PoolableType.Effect, addressValues.attackHitEffectAddress);

                GetAttackHitEffect = () => ResourcesLoader.GetPoolableObject<BattleEffect>(PoolableType.Effect, addressValues.attackHitEffectAddress);
            }
            isPoolObject = true;
        }
        #endregion

        #region 스킬 관련
        public virtual void Skill()
        {
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
    }
}
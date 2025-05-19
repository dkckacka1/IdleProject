using Cysharp.Threading.Tasks;
using IdleProject.Battle.AI;
using IdleProject.Battle.UI;
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
    public partial class CharacterController : MonoBehaviour, ITargetedAble
    {
        public StatSystem statSystem;
        public CharacterState state;

        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationController animController;

        public Transform GetTransform => transform;

        public CharacterUIController characterUI;
        public CharacterAIController characterAI;

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
            state.Initialize();

            SetCharacterData(data);

            SetCharacterAI();
            await SetCharacterUI();
        }

        private void SetAnimationEvent()
        {
            SetBattleAnimEvent();
        }

        protected virtual void SetCharacterData(CharacterData data)
        {
            statSystem.SetStatData(data.stat);
        }

        private void SetCharacterAI()
        {
            BattleManager.Instance.battleEvent.AddListener(characterAI.BattleAction);
        }

        private async UniTask SetCharacterUI()
        {
            await characterUI.SpawnCharacterUI();
            characterUI.SetCharacterUI(statSystem);
            BattleManager.Instance.battleUIEvent.AddListener(characterUI.BattleAction);
        }
        #endregion

        #region 스킬 관련
        public virtual void Skill()
        {
        }
        #endregion

        public static implicit operator Vector3(CharacterController controller) => controller.transform.position;
    }
}
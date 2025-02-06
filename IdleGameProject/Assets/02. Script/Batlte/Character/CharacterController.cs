using IdleProject.Battle.AI;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{
    public enum CharacterState
    {
        None = -1,
        Idle,
        Chase,
        Battle,
        Die,
    }

    [System.Serializable]
    public partial class CharacterController : MonoBehaviour, ITargetedAble
    {
        public StatSystem statSystem;
        public CharacterState currentState;
        public CharacterAIController ai;

        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationController animController;
        

        public Transform GetTransform => transform;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            statSystem = new StatSystem();
            ai = GetComponent<CharacterAIController>();

            animController = new AnimationController(animator, GetComponentInChildren<AnimationEventHandler>());
        }

        private void Start()
        {
            Initialized();
        }

        #region 초기화 부문
        public virtual void Initialized()
        {
            SetStatModifedEvent();
            SetAnimationEvent();
        }
        private void SetStatModifedEvent()
        {
            statSystem.attackRange.Subscribe(value => agent.stoppingDistance = value);
        }

        private void SetAnimationEvent()
        {
            SetBattleAnimEvent();
        }


        public virtual void SetCharacterData(CharacterData data)
        {
            statSystem.SetStatData(data.stat);

            statSystem.movementSpeed.DistinctUntilChanged().Subscribe(SetMovementSpeed);
        }
        #endregion

        #region 사망 관련
        public virtual void Death()
        {
            animController.SetDeath();
        }

        #endregion

        #region 스킬 관련
        public virtual void Skill()
        {
        }
        #endregion

        #region 테스트 항목
        [SerializeField] bool isTest;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && isTest)
            {
                if (statSystem is not null)
                    Gizmos.DrawWireSphere(this.transform.position, statSystem.attackRange.Value);
            }
        } 
        #endregion
    }
}
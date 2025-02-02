using Sirenix.OdinInspector;
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
    public class CharacterController : MonoBehaviour, ITakeDamagedAble
    {
        public StatSystem statSystem;
        public CharacterState currentState;

        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationEventHandler animEventHandler;

        private readonly int skillAnimHash = Animator.StringToHash("Skill");
        private readonly int attackAnimHash = Animator.StringToHash("Attack");
        private readonly int deathAnimHash = Animator.StringToHash("Death");
        private readonly int moveAnimHash = Animator.StringToHash("Move");
        private readonly int idleAnimHash = Animator.StringToHash("Idle");

        public ITargetedAble Target;

        public Transform GetTransform => transform;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            animEventHandler = GetComponentInChildren<AnimationEventHandler>();

            statSystem = new StatSystem();
        }

        private void Start()
        {
            Initialized();
        }

        #region 초기화 부문
        public virtual void Initialized()
        {
            SetAnimationEvent();
        }

        protected virtual void SetAnimationEvent()
        {
        }

        public virtual void SetCharacterData(CharacterData data)
        {
            statSystem.SetStatData(data.stat);

            statSystem.movementSpeed.DistinctUntilChanged().Subscribe(SetMovementSpeed);
            statSystem.attackRange.DistinctUntilChanged().Subscribe(SetAttackRange);
        }
        #endregion

        #region 이동 관련
        public virtual void Move(Vector3 destination)
        {
            animator.SetTrigger(moveAnimHash);
            agent.SetDestination(destination);

            // TODO : 목적지에 도착했을때의 행동 매개변수 정의
            System.IDisposable dispose = null;
            dispose = Observable.EveryFixedUpdate().Where(_ => agent.remainingDistance < statSystem.attackRange.Value).Subscribe(_ =>
            {
                Debug.Log("목표지점 도착");
                animator.SetTrigger(idleAnimHash);
                dispose.Dispose();
            });
        }

        public virtual void SetMovementSpeed(float movementSpeed)
        {
            agent.speed = movementSpeed;
        }
        #endregion

        #region 공격 관련
        public virtual void Attack()
        {
            animator.SetTrigger(attackAnimHash);
        }

        public void Hit(ITakeDamagedAble iTakeDamage)
        {
            iTakeDamage.TakeDamage(10);
        }

        public virtual void TakeDamage(float attackDamage)
        {
            Debug.Log($"{name}이 {attackDamage} 만큼 데미지를 입었습니다.");
        }

        public virtual void SetAttackRange(float attackRange)
        {
            agent.stoppingDistance = attackRange;
        }

        #endregion

        #region 사망 관련
        public virtual void Death()
        {
            animator.SetTrigger(deathAnimHash);
        }

        #endregion

        #region 스킬 관련
        public virtual void Skill()
        {
            animator.SetTrigger(skillAnimHash);
        }
        #endregion

        [SerializeField] bool isTest;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && isTest)
            {
                if (statSystem is not null)
                    Gizmos.DrawWireSphere(this.transform.position, statSystem.attackRange.Value);
            }
        }
    }
}
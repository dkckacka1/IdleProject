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

        public CharacterMovement movement;

        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationController animController;

        public ITargetedAble Target;

        public Transform GetTransform => transform;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            statSystem = new StatSystem();
            movement = new CharacterMovement(agent);
            animController = new AnimationController(animator, GetComponentInChildren<AnimationEventHandler>());
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
            animController.SetMove();

            movement.Move(destination, () =>
            {
                animController.SetIdle();
                Debug.Log("목표지점 도착");
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
            animController.SetAttack();
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
            animController.SetDeath();
        }

        #endregion

        #region 스킬 관련
        public virtual void Skill()
        {
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
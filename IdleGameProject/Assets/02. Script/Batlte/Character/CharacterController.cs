using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{
    public class CharacterController : MonoBehaviour, ITakeDamagedAble
    {
        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        protected AnimationEventHandler animEventHandler;

        private readonly int skillAnimHash = Animator.StringToHash("Skill");
        private readonly int attackAnimHash = Animator.StringToHash("Attack");
        private readonly int deathAnimHash = Animator.StringToHash("Death");
        private readonly int moveAnimHash = Animator.StringToHash("Move");

        public ITargetedAble Target;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            animEventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        private void Start()
        {
            Initialized();
        }

        public virtual void Initialized()
        {
            SetAnimationEvent();
        }

        protected virtual void SetAnimationEvent()
        {

        }


        #region 이동 관련
        public virtual void Move(Vector3 destination)
        {
            animator.SetTrigger(moveAnimHash);
            agent.SetDestination(destination);
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
    }
}
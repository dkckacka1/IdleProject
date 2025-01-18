using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{
    public class CharacterController : MonoBehaviour
    {
        protected Rigidbody rb;
        protected NavMeshAgent agent;
        protected Animator animator;

        private readonly int skillAnimHash = Animator.StringToHash("Skill");
        private readonly int attackAnimHash = Animator.StringToHash("Attack");
        private readonly int deathAnimHash = Animator.StringToHash("Death");
        private readonly int moveAnimHash = Animator.StringToHash("Move");

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            Initialized();
        }

        public virtual void Initialized()
        {

        }


        #region 이동 관련
        public virtual void Move(Vector3 destination)
        {
            animator.SetTrigger(moveAnimHash);
            agent.SetDestination(destination);
        }
        #endregion

        #region Attack
        public virtual void Attack(CharacterController characterController)
        {
            animator.SetTrigger(attackAnimHash);
        }


        #endregion

        #region Death
        public virtual void Death()
        {
            animator.SetTrigger(deathAnimHash);
        }

        #endregion

        #region Skill
        public virtual void Skill()
        {
            animator.SetTrigger(skillAnimHash);
        } 
        #endregion
    }
}
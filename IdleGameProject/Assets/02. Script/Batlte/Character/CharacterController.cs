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

        public virtual void Move(Vector3 destination)
        {
            animator.SetTrigger(moveAnimHash);
            agent.SetDestination(destination);
        }

        public virtual void Attack(CharacterController character)
        {
            animator.SetTrigger(attackAnimHash);
        }

        public virtual void Death()
        {
            animator.SetTrigger(deathAnimHash);
        }

        public virtual void Skill()
        {
            animator.SetTrigger(skillAnimHash);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{

    [System.Serializable]
    public partial class CharacterController : MonoBehaviour, ITargetedAble
    {
        public StatSystem statSystem;

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


        private void SetAnimationEvent()
        {
            SetBattleAnimEvent();
        }

        public virtual void SetCharacterData(CharacterData data)
        {
            statSystem.SetStatData(data.stat);
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
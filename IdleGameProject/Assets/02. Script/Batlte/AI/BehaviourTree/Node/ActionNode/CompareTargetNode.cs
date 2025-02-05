using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CompareTargetNode : ActionNode
    {
        private enum CompareType
        {
            None = -1,
            CompareAttackRange,
        }

        [HideInInspector]
        [SerializeField]
        private CompareType nodeType;
        
        [ShowInInspector]
        private CompareType NodeType
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
                description = GetDescription(nodeType);
            }
        }

        public CompareTargetNode()
        {
            description = GetDescription(nodeType);
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (nodeType)
            {
                case CompareType.CompareAttackRange: return CompareAttackRangeWithDistance();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State CompareAttackRangeWithDistance()
        {
            if (Blackboard_Character.target is null) return State.Failure;

            return 
                Blackboard_Character.Stat.attackRange.Value <= Vector3.Distance(Blackboard_Character.Controller.transform.position, Blackboard_Character.target.GetTransform.position) ? 
                State.Running : 
                State.Success;
        }

        private string GetDescription(CompareType compareType)
        {
            var result = "";

            switch(compareType)
            {
                case CompareType.CompareAttackRange: result = "대상과 자신의 거리와 공격 거리를 비교합니다.";
                        break;
            }

            return result;
        }
    }
}
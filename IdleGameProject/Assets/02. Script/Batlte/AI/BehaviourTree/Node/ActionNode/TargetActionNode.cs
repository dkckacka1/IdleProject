using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace IdleProject.Battle.AI
{
    public class TargetActionNode : ActionNode
    {
        private enum CheckTargetType
        {
            None = -1,
            CheckTarget,
            FoundNearestTarget
        }

        [HideInInspector]
        [SerializeField]
        private CheckTargetType nodeType;

        [ShowInInspector]
        private CheckTargetType NodeType
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

        public TargetActionNode()
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
            switch(nodeType)
            {
                case CheckTargetType.CheckTarget: return CheckTarget();
                case CheckTargetType.FoundNearestTarget: return FindingNearestTarget();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State CheckTarget()
        {
            return Blackboard_Character.target is null ? State.Failure : State.Success;
        }

        private State FindingNearestTarget()
        {
            var targetList = BattleManager.Instance.GetCharacterList(!Blackboard_Character.CharacterAI.isEnemy);

            Blackboard_Character.target = targetList.OrderBy(character => Vector3.Distance(CharacterController.transform.position, character.transform.position)).Single();

            return Blackboard_Character.target is not null ? State.Success : State.Failure;
        }

        private string GetDescription(CheckTargetType type)
        {
            var result = "";

            switch (type)
            {
                case CheckTargetType.CheckTarget:
                    result = "현재 대상이 있는지 확인합니다.";
                    break;
                case CheckTargetType.FoundNearestTarget:
                    result = "가장 가까운 적을 확인해 대상으로 입력합니다.";
                    break;
            }

            return result;
        }
    }
}



using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Engine.AI.BehaviourTree;

namespace IdleProject.Battle.AI
{
    public class TargetActionNode : ActionNode
    {
        private enum CheckTargetType
        {
            None = -1,
            CheckTarget,
            FoundNearestEnemyTarget
        }

        [SerializeField]
        private CheckTargetType nodeType;

        public override string GetTitleName => base.GetTitleName + $"({nodeType})"; 

        private void OnValidate()
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
                case CheckTargetType.FoundNearestEnemyTarget: return FindingNearestEnemyTarget();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State CheckTarget()
        {
            return Target is null ? State.Failure : State.Success;
        }

        private State FindingNearestEnemyTarget()
        {
            var targetList = BattleManager.Instance.GetCharacterList(!Blackboard_Character.CharacterAI.isEnemy);

            Blackboard_Character.Target = targetList.OrderBy(character => Vector3.Distance(Character.transform.position, character.transform.position)).Single();

            return Blackboard_Character.Target is not null ? State.Success : State.Failure;
        }

        private string GetDescription(CheckTargetType type)
        {
            var result = "";

            switch (type)
            {
                case CheckTargetType.CheckTarget:
                    result = "현재 대상이 있는지 확인합니다.";
                    break;
                case CheckTargetType.FoundNearestEnemyTarget:
                    result = "가장 가까운 적을 확인해 대상으로 입력합니다.";
                    break;
            }

            return result;
        }
    }
}



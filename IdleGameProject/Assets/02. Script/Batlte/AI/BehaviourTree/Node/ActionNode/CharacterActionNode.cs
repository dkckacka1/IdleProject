using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CharacterActionNode : ActionNode
    {
        private enum CharacterAction
        {
            CurrentTargetCheck,
            FoundTarget,
            MoveToTarget,
        }

        [ShowInInspector]
        [SerializeField] private CharacterAction actionType;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (actionType)
            {
                case CharacterAction.FoundTarget:
                    return FindingNearestTarget();
                case CharacterAction.CurrentTargetCheck:
                    return CurrentTargetCheck();
                case CharacterAction.MoveToTarget:
                    return MoveToTarget();
            }

            return State.Failure;
        }

        private State CurrentTargetCheck()
        {
            return Blackboard_Character.target is null ? State.Failure : State.Success;
        }

        private State FindingNearestTarget()
        {
            var targetList = BattleManager.Instance.GetCharacterList(!Blackboard_Character.CharacterAI.isEnemy);

            Blackboard_Character.target = targetList.OrderBy(character => Vector3.Distance(CharacterController.transform.position, character.transform.position)).Single();

            return Blackboard_Character.target is not null ? State.Success : State.Failure;
        }

        private State MoveToTarget()
        {
            // TODO : 업데이트 당 한번씩 경로 수정
            if (Blackboard_Character.target is null) return State.Failure;

            CharacterController.Move(Blackboard_Character.target.GetTransform.position);

            return State.Success;
        }
    }
}
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CharacterActionNode : ActionNode
    {
        private enum CharacterAction
        {
            None = -1,
            MoveToTarget,
        }

        [HideInInspector]
        [SerializeField]
        private CharacterAction actionType;

        [ShowInInspector]
        private CharacterAction ActionType
        {
            get
            {
                return actionType;
            }
            set
            {
                actionType = value;
                description = GetDescription(actionType);
            }
        }


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
                case CharacterAction.MoveToTarget:
                    return MoveToTarget();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State MoveToTarget()
        {
            if (Blackboard_Character.target is null) return State.Failure;

            CharacterController.Move(Blackboard_Character.target.GetTransform.position);

            return State.Running;
        }

        private string GetDescription(CharacterAction actionType)
        {
            var result = "";

            switch (actionType)
            {
                case CharacterAction.MoveToTarget:
                    result = "대상을 향해 이동합니다.";
                    break;
            }

            return result;
        }
    }
}
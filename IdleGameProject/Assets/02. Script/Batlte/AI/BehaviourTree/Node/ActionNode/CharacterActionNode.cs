using Engine.AI.BehaviourTree;
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
            AttackToTarget,
        }

        [SerializeField]
        private CharacterAction nodeType;

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
            switch (nodeType)
            {
                case CharacterAction.MoveToTarget: return MoveToTarget();
                case CharacterAction.AttackToTarget: return AttackToTarget();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State MoveToTarget()
        {
            if (Target is null) return State.Failure;

            Character.Move(Target.transform.position);

            return State.Running;
        }

        private State AttackToTarget()
        {
            if (Target is null) return State.Failure;

            Character.Attack();

            return State.Running;
        }

        private string GetDescription(CharacterAction actionType)
        {
            switch (actionType)
            {
                case CharacterAction.MoveToTarget: return "대상을 향해 이동합니다.";
                case CharacterAction.AttackToTarget: return "대상을 공격합니다.";
                default: return "";
            }
        }
    }
}
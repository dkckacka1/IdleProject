using Engine.AI.BehaviourTree;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

using CharacterStatType = IdleProject.Battle.Character.CharacterStatType;

namespace IdleProject.Battle.AI
{
    public class CompareTargetNode : ActionNode
    {
        private enum CompareType
        {
            None = -1,
            TargetInsideInAttackRange,
            IsTargetLive,
        }

        [SerializeField]
        private CompareType nodeType;

        public override string GetSubTitleName => nodeType.ToString();
        public override string GetDescription => GetNodeDescription(nodeType);

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
                case CompareType.TargetInsideInAttackRange: return TargetInsideInAttackRange();
                case CompareType.IsTargetLive: return IsTargetLive();
                default:
                    Debug.LogError("Invaild Node Error");
                    return State.Failure;
            }
        }

        private State TargetInsideInAttackRange()
        {
            if (Target is null) return State.Failure;

            return 
                Character.statSystem.GetStatValue(CharacterStatType.AttackRange) >= Vector3.Distance(Character.transform.position, Target.transform.position) ? 
                State.Success : 
                State.Failure;
        }

        private State IsTargetLive()
        {
            if (Target is null) return State.Failure;

            return Target.statSystem.isLive ? State.Success : State.Failure;
        }

        private string GetNodeDescription(CompareType compareType)
        {
            switch (compareType)
            {
                case CompareType.TargetInsideInAttackRange:
                    return "대상과 자신의 거리가 공격 사거리 이내에 있다면 Success, 밖에 있다면 Failure.";
                case CompareType.IsTargetLive:
                    return "대상이 살아있는지 여부를 확인합니다. 살아있다면 Success, 죽어있다면 Failure";
                default: return "";
            }
        }
    }
}
using Engine.AI.BehaviourTree;
using IdleProject.Battle.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CheckTargetNode : ActionNode
    {
        private enum CheckType
        {
            None = -1,
            CompareStat
        }

        private enum CompareSign
        {
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual,
        }

        [SerializeField]
        private CheckType nodeType;

        #region CompareStat
        [Space(5)]
        [ShowIf("@this.nodeType == CheckType.CompareStat")]
        [SerializeField]
        private CharacterStatType compareStatType;

        [ShowIf("@this.nodeType == CheckType.CompareStat")]
        [SerializeField]
        private CompareSign sign;

        [ShowIf("@this.nodeType == CheckType.CompareStat")]
        [SerializeField]
        private bool isPercent;
        [ShowIf("@this.nodeType == CheckType.CompareStat && this.isPercent")]
        [SerializeField]
        [Range(0, 100)]
        private float comparePercent;
        [ShowIf("@this.nodeType == CheckType.CompareStat && !this.isPercent")]
        [SerializeField]
        private float compareValue; 
        #endregion

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
                case CheckType.CompareStat: return CompareStat();
                default: return State.Failure;
            }
        }

        private State CompareStat()
        {
            if (isPercent)
            // 퍼센트 비교
            {
                float targetPercent = Target.statSystem.GetStatValue(compareStatType) / Target.statSystem.GetStatValue(compareStatType, true);

                switch (sign)
                {
                    case CompareSign.Greater: return targetPercent > comparePercent ? State.Success : State.Failure;
                    case CompareSign.GreaterOrEqual: return targetPercent >= comparePercent ? State.Success : State.Failure;
                    case CompareSign.Less: return targetPercent < comparePercent ? State.Success : State.Failure;
                    case CompareSign.LessOrEqual: return targetPercent <= comparePercent ? State.Success : State.Failure;
                }
            }
            else
                // 값 비교
            {
                var targetValue = Target.statSystem.GetStatValue(compareStatType);

                switch (sign)
                {
                    case CompareSign.Greater: return targetValue > compareValue ? State.Success : State.Failure;
                    case CompareSign.GreaterOrEqual: return targetValue >= compareValue ? State.Success : State.Failure;
                    case CompareSign.Less: return targetValue < compareValue ? State.Success : State.Failure;
                    case CompareSign.LessOrEqual: return targetValue <= compareValue ? State.Success : State.Failure;
                }
            }

            return State.Failure;
        }

        private string GetDescription(CheckType nodeType)
        {
            switch (nodeType)
            {
                default: return "대상의 스탯을 값과 비교합니다. true 면 Success, false 면 Failure";
            }
        }
    }
}
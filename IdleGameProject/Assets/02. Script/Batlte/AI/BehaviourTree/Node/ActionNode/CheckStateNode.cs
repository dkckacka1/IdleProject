using IdleProject.Battle.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CheckStateNode : ActionNode
    {
        private enum CheckType
        {
            None = -1,
            CompareStat,
            CheckState,
        }

        private enum TargetType
        {
            Mine = 0,
            Target,
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

        [SerializeField]
        private TargetType targetType;

        public override string GetTitleName => base.GetTitleName;

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
                case CheckType.CompareStat: return CompareStat();
                case CheckType.CheckState: return CheckState();
                default: return State.Failure;
            }
        }

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

        private State CompareStat()
        {
            var targetController = targetType == TargetType.Mine ? Character : Target;

            if (isPercent)
            // 퍼센트 비교
            {
                float targetPercent = targetController.statSystem.GetStatValue(compareStatType) / targetController.statSystem.GetStatValue(compareStatType, true);

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
                var targetValue = targetController.statSystem.GetStatValue(compareStatType);

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
        #endregion

        #region CheckState
        private enum CheckStateType
        {
            IsLive,
        }

        [Space(5)]
        [ShowIf("@this.nodeType == CheckType.CheckState")]
        [SerializeField]
        private CheckStateType checkStateType;

        private State CheckState()
        {
            var targetController = targetType == TargetType.Mine ? Character : Target;

            switch (checkStateType)
            {
                case CheckStateType.IsLive: return targetController.statSystem.isLive ? State.Success : State.Failure;
                default: return State.Failure;
            }
        }
        #endregion

        private string GetNodeDescription(CheckType nodeType)
        {
            switch (nodeType)
            {
                case CheckType.CompareStat: return $"{targetType}의 스탯을 값과 비교합니다. true 면 Success, false 면 Failure";
                case CheckType.CheckState: return $"{targetType}이 {checkStateType} 상태 인지 확인합니다.. true 면 Success, false 면 Failure";
                default: return $"";
            }
        }
    }
}
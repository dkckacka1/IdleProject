using System;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        private const float AttackRangeCorrectionValue = 0.1f;

        private void SetStatModifedEvent()
        {
            StatSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, SetMovementSpeed);
            StatSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, SetAttackRange);
            BattleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            OnTimeFactorChange(BattleManager.GetCurrentBattleSpeed);
        }

        public virtual void SetMovementSpeed(float movementSpeed)
        {
            Agent.speed = movementSpeed * BattleManager.GetCurrentBattleSpeed;
        }
        private void SetAttackRange(float attackRange)
        {
            Agent.stoppingDistance = attackRange - AttackRangeCorrectionValue;
        }

        public void OnTimeFactorChange(float timeFactor)
        {
            Agent.speed = StatSystem.GetStatValue(CharacterStatType.MovementSpeed) * timeFactor;
        }
    }
}

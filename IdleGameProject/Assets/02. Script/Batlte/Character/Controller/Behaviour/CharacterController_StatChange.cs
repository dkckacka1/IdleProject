using System;
using System.Data;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        private const float ATTACK_RANGE_CORRECTION_VALUE = 0.1f;

        private void SetStatModifyEvent()
        {
            StatSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, ChangeMovementSpeed);
            StatSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, ChangeAttackRange);
            BattleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            OnTimeFactorChange(BattleManager.GetCurrentBattleSpeed);
        }

        public virtual void ChangeMovementSpeed(float movementSpeed)
        {
            Agent.speed = movementSpeed * BattleManager.GetCurrentBattleSpeed;
        }
        private void ChangeAttackRange(float attackRange)
        {
            Agent.stoppingDistance = attackRange - ATTACK_RANGE_CORRECTION_VALUE;
        }

        public void OnTimeFactorChange(float timeFactor)
        {
            Agent.speed = StatSystem.GetStatValue(CharacterStatType.MovementSpeed) * timeFactor;
        }
    }
}

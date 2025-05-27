using System;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        private const float AttackRangeCorrectionValue = 0.1f;

        private void SetStatModifedEvent()
        {
            statSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, SetMovementSpeed);
            statSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, SetAttackRange);
        }

        public virtual void SetMovementSpeed(float movementSpeed)
        {
            agent.speed = movementSpeed;
        }
        private void SetAttackRange(float attackRange)
        {
            agent.stoppingDistance = attackRange - AttackRangeCorrectionValue;
        }
    }
}

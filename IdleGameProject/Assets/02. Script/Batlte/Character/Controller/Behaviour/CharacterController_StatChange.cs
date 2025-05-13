using System;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
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
            agent.stoppingDistance = attackRange;
        }
    }
}

using System;
using System.Data;
using Engine.Core.EventBus;
using IdleProject.Core;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        private const float ATTACK_RANGE_CORRECTION_VALUE = 0.1f;

        public virtual void ChangeMovementSpeed(float movementSpeed)
        {
            Agent.speed = movementSpeed * GetBattleManager.GetCurrentBattleSpeed;
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

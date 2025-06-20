using System;
using System.Data;
using Engine.Core.EventBus;
using IdleProject.Core;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : IEnumEvent<GameStateType>
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

        public void OnEnumChange(GameStateType type)
        {
            switch (type)
            {
                case GameStateType.Play:
                    Agent.enabled = true;
                    break;
                case GameStateType.Pause:
                    Agent.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}

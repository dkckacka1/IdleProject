using System;
using System.Data;
using Engine.Core.EventBus;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController : IEnumEvent<GameStateType>
    {
        private const float ATTACK_RANGE_CORRECTION_VALUE = 0.1f;

        private void SetStatModifyEvent()
        {
            StatSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, ChangeMovementSpeed);
            StatSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, ChangeAttackRange);
            BattleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            _battleManager.GameStateEventBus.PublishEvent(this);
            
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

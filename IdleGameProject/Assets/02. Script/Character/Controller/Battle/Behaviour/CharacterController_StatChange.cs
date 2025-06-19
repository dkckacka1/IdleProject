using System;
using Engine.Core.EventBus;
using IdleProject.Battle;
using IdleProject.Character.Stat;
using IdleProject.Core;

namespace IdleProject.Character
{
    public partial class BattleCharacterController : IEnumEvent<GameStateType>
    {
        private const float ATTACK_RANGE_CORRECTION_VALUE = 0.1f;

        private void PublishStatModifyEvent()
        {
            StatSystem.PublishValueChangedEvent(CharacterStatType.MovementSpeed, ChangeMovementSpeed);
            StatSystem.PublishValueChangedEvent(CharacterStatType.AttackRange, ChangeAttackRange);
            GameManager.GetCurrentSceneManager<BattleManager>().GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.PublishEvent(this);
            OnTimeFactorChange(GameManager.GetCurrentSceneManager<BattleManager>().GetCurrentBattleSpeed);
        }

        public virtual void ChangeMovementSpeed(float movementSpeed)
        {
            Agent.speed = movementSpeed * GameManager.GetCurrentSceneManager<BattleManager>().GetCurrentBattleSpeed;
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

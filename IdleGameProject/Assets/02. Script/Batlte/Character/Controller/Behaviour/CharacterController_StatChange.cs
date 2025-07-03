using IdleProject.Core;
using IdleProject.Core.GameData;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        public virtual void ChangeMovementSpeed(float movementSpeed)
        {
            Agent.speed = movementSpeed * GetBattleManager.GetCurrentBattleSpeed;
        }
        private void ChangeAttackRange(float attackRange)
        {
            Agent.stoppingDistance = attackRange - DataManager.Instance.ConstData.attackRangeCorrectionValue;
        }

        public void OnTimeFactorChange(float timeFactor)
        {
            Agent.speed = StatSystem.GetStatValue(CharacterStatType.MovementSpeed) * timeFactor;
        }


    }
}

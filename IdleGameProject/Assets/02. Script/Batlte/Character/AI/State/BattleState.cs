using System;
using IdleProject.Core;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class BattleState : State
    {
        public BattleState(CharacterController controller, Func<CharacterController> targetController) : base(
            controller, targetController)
        {
        }

        public override void Excute()
        {
            if (!Controller.State.CanAttack) return;

            if (CanUseSKill())
            {
                Controller.Skill();
            }
            else
            {
                Controller.Attack();
            }
        }

        private bool CanUseSKill()
        {
            return BattleManager.Instance<BattleManager>().IsAnyCharacterUsingSkill is false &&
                   Controller.HasSkill && Controller.StatSystem.CanUseSkill && Controller.isNowSkill is false;
        }
    }
}
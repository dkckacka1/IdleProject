using System;
using IdleProject.Battle;
using IdleProject.Core;

namespace IdleProject.Character.AI.State
{
    public class BattleState : State
    {
        public BattleState(BattleCharacterController controller, Func<BattleCharacterController> targetController) : base(
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
            return GameManager.GetCurrentSceneManager<BattleManager>().IsAnyCharacterUsingSkill is false &&
                   Controller.HasSkill && Controller.StatSystem.CanUseSkill && Controller.isNowSkill is false;
        }
    }
}
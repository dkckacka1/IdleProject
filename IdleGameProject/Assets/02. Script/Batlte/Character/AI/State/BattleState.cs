using System;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class BattleState : State
    {
        public BattleState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }
        public override void Excute()
        {
            if (!Controller.State.CanAttack) return;
            
            if (Controller.HasSkill && Controller.StatSystem.CanUseSkill)
            {
                Controller.Skill();
            }
            else
            {
                Controller.Attack();
            }
        }
    }
}

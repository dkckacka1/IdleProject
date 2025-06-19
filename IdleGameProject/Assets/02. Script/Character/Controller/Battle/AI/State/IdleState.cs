using System;

namespace IdleProject.Character.AI.State
{
    public class IdleState : State
    {
        public IdleState(BattleCharacterController controller, Func<BattleCharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
            Controller.Idle();
        }
    }
}
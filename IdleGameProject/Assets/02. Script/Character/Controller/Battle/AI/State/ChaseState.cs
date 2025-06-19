using System;

namespace IdleProject.Character.AI.State
{
    public class ChaseState : State
    {
        public ChaseState(BattleCharacterController controller, Func<BattleCharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
            Controller.Move(Target);
        }
    }
}
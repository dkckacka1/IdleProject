using System;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class IdleState : State
    {
        public IdleState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Execute()
        {
            Controller.Idle();
        }
    }
}
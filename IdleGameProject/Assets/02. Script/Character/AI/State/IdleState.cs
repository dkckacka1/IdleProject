using System;

using CharacterController = IdleProject.Character.CharacterController;

namespace IdleProject.Character.AI.State
{
    public class IdleState : State
    {
        public IdleState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
            Controller.Idle();
        }
    }
}
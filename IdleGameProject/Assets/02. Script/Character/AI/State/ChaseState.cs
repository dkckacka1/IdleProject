using System;

using CharacterController = IdleProject.Character.CharacterController;

namespace IdleProject.Character.AI.State
{
    public class ChaseState : State
    {
        public ChaseState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
            Controller.Move(Target);
        }
    }
}
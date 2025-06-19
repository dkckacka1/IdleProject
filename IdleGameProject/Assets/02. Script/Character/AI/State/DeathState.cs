using System;

using CharacterController = IdleProject.Character.CharacterController;

namespace IdleProject.Character.AI.State
{
    public class DeathState : State
    {
        public DeathState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
        }
    }
}

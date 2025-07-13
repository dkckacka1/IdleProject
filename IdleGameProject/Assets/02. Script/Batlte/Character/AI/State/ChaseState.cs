using System;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class ChaseState : State
    {
        public ChaseState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Execute()
        {
            if (Controller.isNowAttack || Controller.isNowSkill)
                return;
            
            Controller.Move(Target);
        }
    }
}
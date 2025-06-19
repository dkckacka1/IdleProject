using System;

namespace IdleProject.Character.AI.State
{
    public class DeathState : State
    {
        public DeathState(BattleCharacterController controller, Func<BattleCharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
        }
    }
}

using System;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class DeathState : State
    {
        public DeathState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }

        public override void Excute()
        {
        }
    }
}

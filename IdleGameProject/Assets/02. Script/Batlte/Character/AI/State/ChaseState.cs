using System;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
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
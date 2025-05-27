using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public class BattleState : State
    {
        public BattleState(CharacterController controller, Func<CharacterController> targetController) : base(controller, targetController) { }
        public override void Excute()
        {
            if (Controller.state.canAttack)
            {
                if (Controller.statSystem.canUseSkill)
                {
                    Controller.Skill();
                }
                else
                {
                    Controller.Attack();
                }
            }
        }
    }
}

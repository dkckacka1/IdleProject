using UnityEngine;
using IdleProject.Battle.Character;
using Sirenix.OdinInspector;

namespace IdleProject.Battle.AI
{
    public class CheckStateNode : ActionNode
    {

        private CharacterState checkState;

        [ShowInInspector]
        private CharacterState CheckState
        {
            get
            {
                return checkState;
            }
            set
            {
                checkState = value;
                description = $"현재 캐릭터의 State가 [{checkState}] 라면 Success 아닐 시 Failure";
            }
        }
        public CheckStateNode()
        {
            description = "checkState가 현재 캐릭터의 state와 같다면 Success 반환 아닐시 Fail 반환";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (checkState == Blackboard_Character.Controller.currentState)
                return State.Success;

            return State.Failure;
        }
    }
}
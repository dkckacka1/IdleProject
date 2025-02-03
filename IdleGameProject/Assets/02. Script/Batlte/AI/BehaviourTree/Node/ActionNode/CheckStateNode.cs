using UnityEngine;
using IdleProject.Battle.Character;

namespace IdleProject.Battle.AI
{
    public class CheckStateNode : ActionNode
    {
        [SerializeField] private CharacterState checkState;

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
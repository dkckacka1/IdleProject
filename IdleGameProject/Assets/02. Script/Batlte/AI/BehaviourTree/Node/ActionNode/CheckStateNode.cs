using UnityEngine;
using IdleProject.Battle.Character;
using Sirenix.OdinInspector;

namespace IdleProject.Battle.AI
{
    public class CheckStateNode : ActionNode
    {

        [SerializeField]
        private CharacterState checkState;

        public override string GetSubTitleName => checkState.ToString();
        public override string GetDescription => $"현재 캐릭터의 State가 [{checkState}] 라면 Success 아닐 시 Failure";

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
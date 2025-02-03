using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class RepeatUntilFailNode : DecoratorNode
    {
        public RepeatUntilFailNode()
        {
            description = "자식 노드가 Fail이 나올 때 까지 반복";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (child.Update() == State.Failure)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}
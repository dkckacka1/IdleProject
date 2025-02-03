using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class InverterNode : DecoratorNode
    {
        public InverterNode()
        {
            description = "자식 노드의 성공, 실패를 반대로 바꿔주는 노드";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch (child.Update())
            {
                case State.Failure:
                    return State.Success;
                case State.Success:
                    return State.Failure;
            }

            return State.Running;
        }
    }
}
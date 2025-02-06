using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class SequenceNode : CompositeNode
    {
        public SequenceNode()
        {
            description = "자식 노드가 실패할 때까지 좌측에서 우측으로 실행";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            foreach (var node in children)
            {
                switch (node.Update())
                {
                    case State.Running:
                    case State.Success:
                        continue;
                    case State.Failure:
                        return State.Failure;
                }
            }

            return State.Running;
        }
    }
}
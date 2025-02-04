using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode()
        {
            description = "자식 노드가 Success가 나올 때까지 좌측에서 우측으로 실행";
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
                    case State.Success:
                        return State.Success;
                    case State.Running:
                    case State.Failure:
                        continue;
                }
            }

            return State.Failure;
        }
    }
}
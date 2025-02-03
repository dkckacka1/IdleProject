using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class RepeaterNode : DecoratorNode
    {
        public int loopCount = -1;
        public int remainCount = 0;

        public RepeaterNode()
        {
            description = "횟수 만큼 반복하는 노드 (-1 일시 무한 반복)";
        }

        protected override void OnStart()
        {
            remainCount = loopCount;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (remainCount > 0)
            {
                --remainCount;
                child.Update();
            }
            else if(remainCount == 0)
            {
                return State.Success;
            }
            else
            {
                child.Update();
            }
            return State.Running;
        }
    }
}
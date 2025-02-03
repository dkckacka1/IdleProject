using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class SucceederNode : DecoratorNode
    {
        public SucceederNode()
        {
            description = "자식 노드 성공, 실패 여부와 관계없이 성공 반환";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            child.Update();

            return State.Success;
        }
    }
}
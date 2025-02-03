using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class DebugLogNode : Engine.AI.BehaviourTree.ActionNode
    {
        [SerializeField] private string log;

        public DebugLogNode()
        {
            description = "Log의 내용을 디버그 로그로 출력 무조건 Success 반환 (Editor에서만 작동)";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
#if UNITY_EDITOR
            Debug.Log(log);
#endif
            return State.Success;
        }
    }
}
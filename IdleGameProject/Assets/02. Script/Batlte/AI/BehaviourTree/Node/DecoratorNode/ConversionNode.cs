using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class ConversionNode : DecoratorNode
    {
        [SerializeField] private State targetState;
        [SerializeField] private State conversionState;

        private void OnValidate()
        {
            description = $"{targetState} State를 {conversionState} State로 변환합니다.";
        }

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var childState = child.Update();

            return targetState == childState ? conversionState : childState;
        }
    }
}
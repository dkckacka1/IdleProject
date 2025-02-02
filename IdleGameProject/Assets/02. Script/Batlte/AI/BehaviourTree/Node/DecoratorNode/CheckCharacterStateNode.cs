using IdleProject.Battle.Character;
using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CheckCharacterStateNode : DecoratorNode
    {
        [SerializeField] private CharacterState checkState = CharacterState.None;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return Blackboard.controller.currentState == checkState ? State.Success : State.Failure;
        }
    }
}
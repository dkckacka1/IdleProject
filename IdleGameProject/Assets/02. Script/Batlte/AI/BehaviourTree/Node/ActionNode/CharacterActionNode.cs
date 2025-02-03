using UnityEngine;

namespace IdleProject.Battle.AI
{
    public class CharacterActionNode : ActionNode
    {
        private enum CharacterAction
        {
        }

        [SerializeField] private CharacterAction actionType;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            switch(actionType)
            {
                // TODO
            }

            return State.Failure;
        }

    }
}
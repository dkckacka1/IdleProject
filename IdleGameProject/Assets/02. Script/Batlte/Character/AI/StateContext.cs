using UnityEngine;

namespace IdleProject.Battle.AI.State
{
    public class StateContext
    {
        private IState currentState;

        public StateContext(IState defaultState)
        {
            this.currentState = defaultState;
        }

        public void ExcuteState()
        {
            currentState?.Excute();
        }

        public void ChangeState(IState state)
        {
            currentState = state;
        }

        public string CurrentStateToString()
        {
            return currentState.GetType().Name;
        }
    }
}
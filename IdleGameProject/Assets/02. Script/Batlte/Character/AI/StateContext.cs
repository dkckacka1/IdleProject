namespace IdleProject.Battle.AI.State
{
    public class StateContext
    {
        private IState _currentState;

        public StateContext(IState defaultState)
        {
            this._currentState = defaultState;
        }

        public void ExcuteState()
        {
            _currentState?.Excute();
        }

        public void ChangeState(IState state)
        {
            _currentState = state;
        }

        public string CurrentStateToString()
        {
            return _currentState.GetType().Name;
        }
    }
}
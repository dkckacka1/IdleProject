using System;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public abstract class State : IState
    {
        private readonly Func<CharacterController> _targetController;

        protected CharacterController Target => _targetController?.Invoke();

        protected CharacterController Controller { get; }

        protected State(CharacterController controller, Func<CharacterController> targetController)
        {
            Controller = controller;
            _targetController = targetController;
        }

        public abstract void Execute();
    }
}


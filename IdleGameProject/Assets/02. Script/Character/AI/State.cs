using System;
using UnityEngine;

using CharacterController = IdleProject.Character.CharacterController;

namespace IdleProject.Character.AI.State
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

        public abstract void Excute();
    }
}


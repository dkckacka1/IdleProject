using System;
using UnityEngine;

namespace IdleProject.Character.AI.State
{
    public abstract class State : IState
    {
        private readonly Func<BattleCharacterController> _targetController;

        protected BattleCharacterController Target => _targetController?.Invoke();

        protected BattleCharacterController Controller { get; }

        protected State(BattleCharacterController controller, Func<BattleCharacterController> targetController)
        {
            Controller = controller;
            _targetController = targetController;
        }

        public abstract void Excute();
    }
}


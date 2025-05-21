using System;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI.State
{
    public abstract class State : IState
    {
        private CharacterController controller;
        private Func<CharacterController> targetController;

        protected CharacterController Target => targetController?.Invoke();

        protected CharacterController Controller { get => controller; }

        protected State(CharacterController controller, Func<CharacterController> targetController)
        {
            this.controller = controller;
            this.targetController = targetController;
        }

        public abstract void Excute();
    }
}


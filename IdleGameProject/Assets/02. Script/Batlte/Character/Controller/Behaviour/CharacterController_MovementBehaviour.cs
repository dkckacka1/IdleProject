using System;
using UniRx;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {

        private IDisposable moveEndCallback = null;

        public virtual void Move(Vector3 destination)
        {
            AnimController.SetMove();

            Move(destination, () =>
            {
                AnimController.SetIdle();
            });
        }

        public void Move(Vector3 destination, Action moveEndAction)
        {
            Agent?.SetDestination(destination);
        }

        public void Stop()
        {
            Agent.ResetPath();
        }
    }
}
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
            animController.SetMove();

            Move(destination, () =>
            {
                animController.SetIdle();
            });
        }

        public void Move(Vector3 destination, Action moveEndAction)
        {
            agent?.SetDestination(destination);
        }

        public void Stop()
        {
            agent.ResetPath();
        }
    }
}
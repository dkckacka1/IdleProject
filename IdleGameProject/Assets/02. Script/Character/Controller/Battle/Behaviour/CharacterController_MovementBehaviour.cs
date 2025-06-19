using System;
using UnityEngine;

namespace IdleProject.Character
{
    public partial class BattleCharacterController
    {
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
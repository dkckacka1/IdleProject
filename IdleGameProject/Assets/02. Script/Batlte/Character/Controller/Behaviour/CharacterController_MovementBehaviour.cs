using System;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
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

        public void StopMove()
        {
            Agent.ResetPath();
        }
    }
}
using System;
using UniRx;
using UnityEngine;

namespace IdleProject.Battle.Character
{
    public partial class CharacterController
    {
        public bool canMove = true;

        private IDisposable moveEndCallback = null;

        public virtual void Move(Vector3 destination)
        {
            animController.SetMove();
            currentState = CharacterState.Chase;

            Move(destination, () =>
            {
                animController.SetIdle();
            });
        }

        public virtual void SetMovementSpeed(float movementSpeed)
        {
            agent.speed = movementSpeed;
        }

        public void Move(Vector3 destination, Action moveEndAction)
        {
            agent?.SetDestination(destination);

            SetMoveEndAction(moveEndAction);
        }

        public void Stop()
        {
            agent.ResetPath();
        }

        private void SetMoveEndAction(Action moveEndAction)
        {
            if (moveEndCallback is not null)
            {
                moveEndCallback.Dispose();
            }

            moveEndCallback = Observable.EveryFixedUpdate().Where(_ => agent.remainingDistance < agent.stoppingDistance).Subscribe(_ =>
            {
                agent.ResetPath();
                moveEndAction?.Invoke();
                moveEndCallback.Dispose();
            });
        }
    }
}
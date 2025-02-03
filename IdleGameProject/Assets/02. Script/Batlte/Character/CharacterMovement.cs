using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace IdleProject.Battle.Character
{
    public class CharacterMovement
    {
        NavMeshAgent agent;

        public bool canMove = true;

        private IDisposable moveEndCallback = null;

        public CharacterMovement(NavMeshAgent agent)
        {
            this.agent = agent;
        }


        public void Move(Vector3 destination, Action moveEndAction)
        {
            agent?.SetDestination(destination);

            SetMoveEndAction(moveEndAction);
        }

        public void Stop()
        {
            agent.isStopped = false;
        }

        private void SetMoveEndAction(Action moveEndAction)
        {
            if (moveEndCallback is not null)
            {
                moveEndCallback.Dispose();
            }

            moveEndCallback = Observable.EveryFixedUpdate().Where(_ => agent.remainingDistance < agent.stoppingDistance).Subscribe(_ =>
            {
                moveEndAction?.Invoke();
                moveEndCallback.Dispose();
            });
        }
    }
}
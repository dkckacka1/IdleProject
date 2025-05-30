using System;
using System.Linq;
using UnityEngine;

using Engine.AI.BehaviourTree;
using Engine.Core.EventBus;
using IdleProject.Battle.AI.State;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI
{
    public enum CharacterAIType
    {
        Playerable,
        Enemy,
        //Ally,
        //Neutral,
    }


    [RequireComponent(typeof(CharacterController))]
    public class CharacterAIController : MonoBehaviour, IEnumEvent<BattleStateType>
    {
        public CharacterAIType aiType;

        private CharacterController controller;
        private CharacterController currentTarget;

        private StateContext context;
        private IdleState IdleState;
        private ChaseState chaseState;
        private DeathState deathState;
        private BattleState battleState;

        private void Awake()
        {
            Initialized();
        }

        protected virtual void Initialized()
        {
            controller = GetComponent<CharacterController>();
            controller.GetTargetCharacter = GetTargetController;

            IdleState = new IdleState(controller, GetTargetController);
            chaseState = new ChaseState(controller, GetTargetController);
            deathState = new DeathState(controller, GetTargetController);
            battleState = new BattleState(controller, GetTargetController);
            context = new StateContext(IdleState);
        }

        public void OnBatteEvent()
        {
            context.ChangeState(CheckState());
            context.ExcuteState();
        }

        public void OnWinEvent()
        {
        }

        public void OnDefeatEvent()
        {
        }

        private IState CheckState()
        {
            if (controller.State.isDead)
                return deathState;

            currentTarget = GetNealyTarget();
            if (currentTarget is not null)
            {
                if (Vector3.Distance(currentTarget, controller) >= controller.StatSystem.GetStatValue(Character.CharacterStatType.AttackRange))
                {
                    return chaseState;
                }
                else
                {
                    return battleState;
                }
            }

            return IdleState;
        }

        private CharacterController GetTargetController()
        {
            return currentTarget;
        }

        private CharacterController GetNealyTarget()
        {
            var enemyCharacterList = BattleManager.Instance.GetCharacterList(EnemyType(), character => character.StatSystem.isLive);
            var target = enemyCharacterList.OrderBy(character => Vector3.Distance(character.transform.position, controller.transform.position)).FirstOrDefault();
            return target;
        }

        private CharacterAIType EnemyType()
        {
            return (aiType == CharacterAIType.Playerable) ? CharacterAIType.Enemy : CharacterAIType.Playerable;
        }

        public void OnChangeBatteState(BattleStateType battleState)
        {
            switch (battleState)
            {
                case BattleStateType.Ready:
                    break;
                case BattleStateType.Battle:
                    break;
                case BattleStateType.Skill:
                    break;
                case BattleStateType.Win:
                    OnWinEvent();
                    break;
                case BattleStateType.Defeat:
                    OnDefeatEvent();;
                    break;
            }
        }

        public void OnEnumChange<T>(T type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                    break;
                case BattleStateType.Battle:
                    break;
                case BattleStateType.Skill:
                    break;
                case BattleStateType.Win:
                    OnWinEvent();
                    break;
                case BattleStateType.Defeat:
                    OnDefeatEvent();;
                    break;
            }
        }
    }
}
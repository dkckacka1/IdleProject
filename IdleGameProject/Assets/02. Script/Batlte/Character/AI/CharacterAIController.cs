using System;
using System.Linq;
using UnityEngine;

using Engine.AI.BehaviourTree;
using Engine.Core.EventBus;
using IdleProject.Battle.AI.State;
using Zenject;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI
{
    public enum CharacterAIType
    {
        Player,
        Enemy,
        //Ally,
        //Neutral,
    }


    [RequireComponent(typeof(CharacterController))]
    public class CharacterAIController : MonoBehaviour
    {
        [Inject]
        private BattleManager _battleManager;
        
        public CharacterAIType aiType;

        private CharacterController _controller;
        private CharacterController _currentTarget;

        private StateContext _context;
        private IdleState _idleState;
        private ChaseState _chaseState;
        private DeathState _deathState;
        private BattleState _battleState;
        
        [Inject]
        protected virtual void Initialized(CharacterController controller, CharacterAIType aiType)
        {
            _controller = controller;
            this.aiType = aiType;
            _controller.GetTargetCharacter = GetTargetController;

            _battleManager.BattleObjectEventDic[BattleObjectType.Character].AddListener(OnBatteEvent);
            
            _idleState = new IdleState(_controller, GetTargetController);
            _chaseState = new ChaseState(_controller, GetTargetController);
            _deathState = new DeathState(_controller, GetTargetController);
            _battleState = new BattleState(_controller, GetTargetController);
            _context = new StateContext(_idleState);
        }

        public void OnBatteEvent()
        {
            _context.ChangeState(CheckState());
            _context.ExcuteState();
        }

        private IState CheckState()
        {
            if (_controller.State.IsDead)
                return _deathState;

            _currentTarget = GetNealyTarget();
            if (_currentTarget is not null)
            {
                if (Vector3.Distance(_currentTarget, _controller) >= _controller.StatSystem.GetStatValue(Character.CharacterStatType.AttackRange))
                {
                    return _chaseState;
                }
                else
                {
                    return _battleState;
                }
            }

            return _idleState;
        }

        private CharacterController GetTargetController()
        {
            return _currentTarget;
        }

        private CharacterController GetNealyTarget()
        {
            var enemyCharacterList = _battleManager.GetCharacterList(EnemyType()).Where(character => character.StatSystem.IsLive);
            var target = enemyCharacterList.OrderBy(character => Vector3.Distance(character.transform.position, _controller.transform.position)).FirstOrDefault();
            return target;
        }

        private CharacterAIType EnemyType()
        {
            return (aiType == CharacterAIType.Player) ? CharacterAIType.Enemy : CharacterAIType.Player;
        }
    }
}
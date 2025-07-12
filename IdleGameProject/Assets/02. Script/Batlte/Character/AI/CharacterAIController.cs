using System.Linq;
using IdleProject.Battle.AI.State;
using IdleProject.Battle.Character.EventGroup;
using IdleProject.Core;
using UnityEngine;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI
{



    [RequireComponent(typeof(CharacterController))]
    public class CharacterAIController : MonoBehaviour, IEventGroup<BattleManager>
    {
        public CharacterAIType aiType;

        private CharacterController _controller;
        private CharacterController _currentTarget;

        private StateContext _context;
        private IdleState _idleState;
        private ChaseState _chaseState;
        private DeathState _deathState;
        private BattleState _battleState;

        private BattleManager _battleManager;

        private BattleManager BattleManager
        {
            get => _battleManager ??= BattleManager.Instance<BattleManager>();
        }
        
        public CharacterAIType GetAllyType =>
            (aiType == CharacterAIType.Ally) ? CharacterAIType.Ally : CharacterAIType.Enemy;
        public CharacterAIType GetEnemyType =>
            (aiType == CharacterAIType.Ally) ? CharacterAIType.Enemy : CharacterAIType.Ally;
        
        public void Initialized()
        {
            _controller = GetComponent<CharacterController>();
            _controller.GetTargetCharacter = GetTargetController;

            _idleState = new IdleState(_controller, GetTargetController);
            _chaseState = new ChaseState(_controller, GetTargetController);
            _deathState = new DeathState(_controller, GetTargetController);
            _battleState = new BattleState(_controller, GetTargetController);
            _context = new StateContext(_idleState);
        }

        public void OnBattleEvent()
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
                if (CharacterController.IsTargetInsideAttackRange(_controller, _currentTarget))
                {
                    return _battleState;
                }
                else
                {
                    return _chaseState;
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
            return (aiType == CharacterAIType.Ally) ? CharacterAIType.Enemy : CharacterAIType.Ally;
        }

        public void Publish(BattleManager publisher)
        {
            publisher.BattleObjectEventDic[BattleObjectType.Character].AddListener(OnBattleEvent);
        }

        public void UnPublish(BattleManager publisher)
        {
            publisher.BattleObjectEventDic[BattleObjectType.Character].RemoveListener(OnBattleEvent);
        }
    }
}
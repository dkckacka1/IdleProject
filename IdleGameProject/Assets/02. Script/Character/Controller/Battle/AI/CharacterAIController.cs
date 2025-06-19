using System.Linq;
using UnityEngine;
using IdleProject.Battle;
using IdleProject.Character.AI.State;
using IdleProject.Character.Stat;
using IdleProject.Core;

namespace IdleProject.Character.AI
{
    public enum CharacterAIType
    {
        Player,
        Enemy,
        //Ally,
        //Neutral,
    }


    [RequireComponent(typeof(BattleCharacterController))]
    public class CharacterAIController : MonoBehaviour
    {
        public CharacterAIType aiType;

        private BattleCharacterController _controller;
        private BattleCharacterController _currentTarget;

        private StateContext _context;
        private IdleState _idleState;
        private ChaseState _chaseState;
        private DeathState _deathState;
        private BattleState _battleState;
        
        public void Initialized()
        {
            _controller = GetComponent<BattleCharacterController>();
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
                if (Vector3.Distance(_currentTarget, _controller) >= _controller.StatSystem.GetStatValue(CharacterStatType.AttackRange))
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

        private BattleCharacterController GetTargetController()
        {
            return _currentTarget;
        }

        private BattleCharacterController GetNealyTarget()
        {
            
            
            var enemyCharacterList = GameManager.GetCurrentSceneManager<BattleManager>().GetCharacterList(EnemyType()).Where(character => character.StatSystem.IsLive);
            var target = enemyCharacterList.OrderBy(character => Vector3.Distance(character.transform.position, _controller.transform.position)).FirstOrDefault();
            return target;
        }

        private CharacterAIType EnemyType()
        {
            return (aiType == CharacterAIType.Player) ? CharacterAIType.Enemy : CharacterAIType.Player;
        }
    }
}
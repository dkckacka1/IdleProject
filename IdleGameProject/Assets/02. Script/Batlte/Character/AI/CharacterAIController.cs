using UnityEngine;

using Engine.AI.BehaviourTree;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using System.Linq;

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
    public class CharacterAIController : MonoBehaviour
    {
        public CharacterAIType aiType;

        private CharacterController controller;
        private CharacterController currentTarget;

        private void Awake()
        {
            Initialized();
        }

        protected virtual void Initialized()
        {
            controller = GetComponent<CharacterController>();
            controller.GetTargetCharacter = GetTargetController;
        }

        public void BattleAction()
        {
            currentTarget = GetNealyTarget();
            controller.Move(currentTarget);

            if (Vector3.Distance(currentTarget, controller) < controller.statSystem.GetStatValue(Character.CharacterStatType.AttackRange))
            {
                controller.Attack();
            }
        }

        private CharacterController GetTargetController()
        {
            return currentTarget;
        }

        private CharacterController GetNealyTarget()
        {
            var enemyCharacterList = BattleManager.Instance.GetCharacterList(EnemyType());
            var target = enemyCharacterList.OrderBy(character => Vector3.Distance(character.transform.position, controller.transform.position)).FirstOrDefault();
            return target;
        }

        private CharacterAIType EnemyType()
        {
            return (aiType == CharacterAIType.Playerable) ? CharacterAIType.Enemy : CharacterAIType.Playerable;
        }
    }
}
using UnityEngine;

using Engine.AI.BehaviourTree;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.AI
{
    public class CharacterAIController : BehaviourTreeRunner, BattleAIAble
    {
        public bool isEnemy;
        public bool isPlay = false;

        public CharacterController target;

        protected override void Initialized()
        {
            tree = tree.Clone();
            tree.Bind();

            tree.SetBlackboard(new Blackboard_Character(GetComponent<CharacterController>(), this));

            BattleManager.Instance.battleAIList.Add(this);

            if (isEnemy)
            {
                BattleManager.Instance.enemyCharacterList.Add(GetComponent<CharacterController>());
            }
            else
            {
                BattleManager.Instance.playerCharacterList.Add(GetComponent<CharacterController>());
            }

        }

        public void UpdateAI()
        {
            tree.Update();
        }

        public void PauseAI()
        {
            tree.Pause();
        }

        public void PlayAI()
        {
            tree.Play();
        }
    }
}
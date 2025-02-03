using UnityEngine;

using Engine.AI.BehaviourTree;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using Sirenix.OdinInspector;
using IdleProject.Battle.Character;

namespace IdleProject.Battle.AI
{
    public class CharacterAIController : BehaviourTreeRunner
    {
        public bool isEnemy;
        public bool isPlay = false;

        public ITargetedAble target;

        protected override void Initialized()
        {
            tree = tree.Clone();
            tree.Bind();

            tree.SetBlackboard(new Blackboard_Character(GetComponent<CharacterController>(), this));
        }

        private void Update()
        {
            if (isPlay)
            {
                tree.Update();
            }
        }

        [Button]
        public void Play()
        {
            isPlay = !isPlay;
        }
    }
}
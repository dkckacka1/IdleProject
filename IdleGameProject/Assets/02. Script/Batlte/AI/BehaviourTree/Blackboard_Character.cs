using Engine.AI.BehaviourTree;

using IdleProject.Battle.Character;

namespace IdleProject.Battle.AI
{
    [System.Serializable]
    public class Blackboard_Character : Blackboard
    {
        public CharacterController Controller { get; private set; }
        public StatSystem Stat { get; private set; }
        public CharacterAIController CharacterAI { get; private set; }

        public Blackboard_Character(CharacterController controller, CharacterAIController characterAI)
        {
            Controller = controller;
            Stat = controller.statSystem;
            CharacterAI = characterAI;
        }
    }
}
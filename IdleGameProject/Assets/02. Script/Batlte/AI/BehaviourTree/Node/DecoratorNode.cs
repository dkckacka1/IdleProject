
namespace IdleProject.Battle.AI
{
    public abstract class DecoratorNode : Engine.AI.BehaviourTree.DecoratorNode
    {
        private new Blackboard_CharacterController blackboard;
        protected Blackboard_CharacterController Blackboard
        {
            get
            {
                if (blackboard is null)
                    TryGetBlackboard(out blackboard);

                return blackboard;
            }
        }   
    }
}
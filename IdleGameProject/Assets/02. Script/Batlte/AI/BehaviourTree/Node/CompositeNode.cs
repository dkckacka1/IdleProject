namespace IdleProject.Battle.AI
{
    public abstract class CompositeNode : Engine.AI.BehaviourTree.CompositeNode
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
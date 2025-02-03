namespace IdleProject.Battle.AI
{
    public abstract class CompositeNode : Engine.AI.BehaviourTree.CompositeNode
    {
        private Blackboard_Character blackboard_Character;
        protected Blackboard_Character Blackboard_Character
        {
            get
            {
                if (blackboard_Character is null)
                    TryGetBlackboard(out blackboard_Character);

                return blackboard_Character;
            }
        }
    }
}
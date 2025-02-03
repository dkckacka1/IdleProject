namespace IdleProject.Battle.AI
{
    public abstract class ActionNode : Engine.AI.BehaviourTree.ActionNode
    {
        private Blackboard_Character blackboard_Character;
        protected Blackboard_Character Blackboard_Character
        {
            get
            {
                if(blackboard_Character is null)
                    TryGetBlackboard(out blackboard_Character);

                return blackboard_Character;
            }
        }
    }
}
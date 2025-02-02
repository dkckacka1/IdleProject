namespace IdleProject.Battle.AI
{
    public abstract class ActionNode : Engine.AI.BehaviourTree.ActionNode
    {
        private new Blackboard_CharacterController blackboard;
        protected Blackboard_CharacterController Blackboard
        {
            get
            {
                if(blackboard is null)
                    TryGetBlackboard(out blackboard);

                return blackboard;
            }
        }
    }
}
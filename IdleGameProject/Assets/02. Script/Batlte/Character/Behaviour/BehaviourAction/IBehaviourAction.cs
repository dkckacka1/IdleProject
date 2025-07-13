namespace IdleProject.Battle.Character.Behaviour.SkillAction
{
    public interface IBehaviourAction
    {
        public void ActionExecute(bool isSkillBehaviour);
    }

    public interface ITargetChangeBehaviourAction : IBehaviourAction
    {
        public void SetTarget(CharacterController target);
    }
}
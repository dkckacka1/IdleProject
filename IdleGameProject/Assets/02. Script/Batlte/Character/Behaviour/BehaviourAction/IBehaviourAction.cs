namespace IdleProject.Battle.Character.Behaviour.SkillAction
{
    public interface IBehaviourAction
    {
        public void SetTarget(CharacterController target);
        
        public void ActionExecute(bool isSkillBehaviour);
    }
}
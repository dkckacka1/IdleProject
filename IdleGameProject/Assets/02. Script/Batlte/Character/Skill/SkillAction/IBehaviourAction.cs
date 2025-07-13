namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public interface IBehaviourAction
    {
        public void SetTarget(CharacterController target);
        
        public void ActionExecute(bool isSkillBehaviour);
    }
}
namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public interface ISkillAction
    {
        public void SetTarget(CharacterController target);
        
        public void ActionExecute();
    }
}
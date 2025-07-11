namespace IdleProject.Battle.Character.Skill.SkillRange
{
    public abstract class SkillRange : ISkillRange
    {
        protected CharacterController Controller;

        protected SkillRange(CharacterController controller)
        {
            Controller = controller;
        }

        public abstract bool GetInRange(CharacterController target);
    }
}
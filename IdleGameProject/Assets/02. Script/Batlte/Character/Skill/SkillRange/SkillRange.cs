namespace IdleProject.Battle.Character.Skill.SkillRange
{
    public abstract class SkillRange : ISkillRange
    {
        protected readonly CharacterController Controller;

        protected SkillRange(CharacterController controller)
        {
            Controller = controller;
        }


        public abstract bool GetInRange(CharacterController target);
    }
}
using IdleProject.Core;
using IdleProject.Data.StaticData.Skill;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class AttackAction : SkillAction
    {
        private bool _canCritical;
        private readonly float _attackValue;
        
        public AttackAction(AttackActionData actionData, CharacterController controller) : base(actionData, controller)
        {
            _canCritical = actionData.canCritical;
            _attackValue = actionData.attackValue;
        }

        public override void ActionExecute()
        {
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * _attackValue;
            
            foreach (var target in GetTargetList.Invoke())
            {
                Controller.Hit(target, attackDamage);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.StaticData.Skill;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class ProjectileAction : SkillAction
    {
        private List<ISkillAction> _onHitActionList;
        
        public ProjectileAction(ProjectileActionData actionData, CharacterController controller, CharacterController currentTarget) : base(actionData, controller, currentTarget)
        {
            // TODO
        }

        public override void ActionExecute()
        {
            // TODO 
            foreach (var target in GetTargetList.Invoke())
            {
                // 대상에게 발사
                
                
                // 맞추면 OnHitAction 발동
                foreach (var onHitAction in _onHitActionList)
                {
                    onHitAction.SetTarget(target);
                    onHitAction.ActionExecute();
                }
            }
        }
    }
}
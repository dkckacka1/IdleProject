using System.Collections.Generic;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class ProjectileAction : SkillAction
    {
        private List<ISkillAction> _onHitActionList;
        
        public ProjectileAction(Data.SkillData.ProjectileSkillActionData skillActionData, CharacterController controller) : base(skillActionData, controller)
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
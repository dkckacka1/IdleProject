using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character.Skill.SkillAction.Implement;
using IdleProject.Battle.Character.Skill.SkillTargeting;
using IdleProject.Core;
using IdleProject.Data.StaticData.Skill;

namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public abstract class SkillAction : ISkillAction
    {
        protected readonly CharacterController Controller;
        public CharacterController CurrentTarget;

        protected readonly Func<List<CharacterController>> GetTargetList;

        
        protected SkillAction(SkillActionData actionData, CharacterController controller)
        {
            Controller = controller;

            GetTargetList = () => GetTarget(actionData.skillTargetList.Select(targetingData =>
                SkillTargeting.SkillTargeting.GetSkillTargeting(controller, targetingData)));
        }
        
        
        protected SkillAction(SkillActionData actionData, CharacterController controller, CharacterController currentTarget) : this(actionData, controller)
        {
        }

        
        
        private List<CharacterController> GetTarget(IEnumerable<ISkillTargeting> skillTargetList)
        {
            var characterList = GameManager.GetCurrentSceneManager<BattleManager>().GetCharacterList();

            return skillTargetList.Aggregate(characterList, (current, skillTarget) => current.Where(target => skillTarget.TargetingCharacterList(target, CurrentTarget))
                .ToList());
        }

        
        public void SetTarget(CharacterController target)
        {
            CurrentTarget = target;
        }

        // 액션을 수행합니다.
        public abstract void ActionExecute();

        
        public static ISkillAction GetSkillAction(SkillActionData actionData, CharacterController controller, CharacterController currentTarget)
        {
            return actionData switch
            {
                // 공격 수행
                AttackActionData attackActionData => throw new NotImplementedException(),
                // 버프 발동
                BuffActionData buffActionData => throw new NotImplementedException(),
                // 이펙트 호출
                EffectActionData effectActionData => throw new NotImplementedException(),
                // 투사체 발사
                ProjectileActionData projectileActionData => new ProjectileAction(projectileActionData, controller,currentTarget),
                _ => throw new ArgumentOutOfRangeException(nameof(actionData))
            };
        }

    }
}
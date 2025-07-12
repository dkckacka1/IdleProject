using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character.Skill.SkillAction.Implement;
using IdleProject.Battle.Character.Skill.SkillTargeting;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public abstract class SkillAction : ISkillAction
    {
        protected readonly CharacterController Controller;

        protected CharacterController CurrentTarget;
        protected Func<List<CharacterController>> GetTargetList;

        private readonly IEnumerable<ISkillTargeting> _targetings;

        protected SkillAction(SkillActionData skillActionData, CharacterController controller)
        {
            Controller = controller;

            if (skillActionData != null)
                _targetings = skillActionData.skillTargetList?.Select(targetingData =>
                    SkillTargeting.SkillTargeting.GetSkillTargeting(controller, targetingData));

            SetTarget(Controller);
        }

        public void SetTarget(CharacterController target)
        {
            CurrentTarget = target;
            GetTargetList = () => GetTarget(_targetings, target);
        }

        
        private List<CharacterController> GetTarget(IEnumerable<ISkillTargeting> skillTargetList, CharacterController targetController)
        {
            var characterList = BattleManager.Instance<BattleManager>().GetCharacterList();
            var targetList = new List<CharacterController>() {targetController};

            foreach (var targeting in skillTargetList)
            {
                targetList = targeting.GetTargetingCharacterList(targetController, characterList, targetList);
            }
            
            return targetList;
        }

        // 액션을 수행합니다.
        public abstract void ActionExecute();


        public static ISkillAction GetSkillAction(SkillActionData skillActionData, CharacterController controller)
        {
            return skillActionData switch
            {
                // 공격 수행
                AttackSkillActionData attackActionData => new AttackAction(attackActionData, controller),
                // 버프 발동
                BuffSkillActionData buffActionData => throw new NotImplementedException(),
                // 이펙트 호출
                EffectSkillActionData effectActionData => new EffectAction(effectActionData, controller),
                // 투사체 발사
                ProjectileSkillActionData projectileActionData =>
                    new ProjectileAction(projectileActionData, controller),
                _ => throw new ArgumentOutOfRangeException(nameof(skillActionData))
            };
        }
    }
}
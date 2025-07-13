using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character.Skill.SkillAction.Implement;
using IdleProject.Battle.Character.Skill.SkillTargeting;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction
{
    public abstract class BehaviourAction : IBehaviourAction
    {
        protected readonly CharacterController Controller;

        protected CharacterController CurrentTarget;
        protected Func<List<CharacterController>> GetTargetList;

        private readonly IEnumerable<IBehaviourTargeting> _targetings;

        protected BehaviourAction(SkillActionData skillActionData, CharacterController controller)
        {
            Controller = controller;

            if (skillActionData != null)
                _targetings = skillActionData.skillTargetList?.Select(targetingData =>
                    SkillTargeting.BehaviourTargeting.GetSkillTargeting(controller, targetingData));

            SetTarget(Controller);
        }

        public void SetTarget(CharacterController target)
        {
            CurrentTarget = target;
            GetTargetList = () => GetTarget(_targetings, target);
        }

        
        private List<CharacterController> GetTarget(IEnumerable<IBehaviourTargeting> skillTargetList, CharacterController targetController)
        {
            var characterList = GameManager.GetCurrentSceneManager<BattleManager>().GetCharacterList();
            var targetList = new List<CharacterController>() {targetController};

            foreach (var targeting in skillTargetList)
            {
                targetList = targeting.GetTargetingCharacterList(targetController, characterList, targetList);
            }
            
            return targetList;
        }

        // 액션을 수행합니다.
        public abstract void ActionExecute(bool isSkillBehaviour);


        public static IBehaviourAction GetSkillAction(SkillActionData skillActionData, CharacterController controller)
        {
            return skillActionData switch
            {
                // 공격 수행
                AttackSkillActionData attackActionData => new AttackAction(attackActionData, controller),
                // 버프 발동
                BuffSkillActionData buffActionData => new BuffAction(buffActionData, controller),
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
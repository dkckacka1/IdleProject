using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character.Behaviour.SkillAction.Implement;
using IdleProject.Battle.Character.Behaviour.Targeting;
using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction
{
    public abstract class BehaviourAction : ITargetChangeBehaviourAction
    {
        protected readonly CharacterController Controller;

        protected Func<List<CharacterController>> GetTargetList;

        private readonly IEnumerable<IBehaviourTargeting> _targetList;

        protected BehaviourAction(BehaviourActionData behaviourActionData, CharacterController controller)
        {
            Controller = controller;

            if (behaviourActionData != null)
                _targetList = behaviourActionData.skillTargetList?.Select(targetingData =>
                    BehaviourTargeting.GetSkillTargeting(controller, targetingData));

            SetTarget(Controller);
        }

        public void SetTarget(CharacterController target)
        {
            GetTargetList = () => GetTarget(_targetList, target);
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


        public static IBehaviourAction GetSkillAction(BehaviourActionData behaviourActionData, CharacterController controller)
        {
            return behaviourActionData switch
            {
                // 공격 수행
                AttackBehaviourActionData attackActionData => new AttackAction(attackActionData, controller),
                // 버프 발동
                BuffBehaviourActionData buffActionData => new BuffAction(buffActionData, controller),
                // 이펙트 호출
                EffectBehaviourActionData effectActionData => new EffectAction(effectActionData, controller),
                // 투사체 발사
                ProjectileBehaviourActionData projectileActionData => new ProjectileAction(projectileActionData, controller),
                // 스탯 변경
                StatChangeActionData statChangeActionData => new StatChangeAction(statChangeActionData, controller),
                // 사운드 호출
                SoundBehaviourActionData soundActionData => new SoundAction(soundActionData),
                _ => throw new ArgumentOutOfRangeException(nameof(behaviourActionData))
            };
        }
    }
}
using System;
using IdleProject.Battle.Effect;
using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class EffectAction : BehaviourAction
    {
        private readonly BehaviourEffectData _effectData;
        private readonly Func<BattleEffect> _getBattleEffect;

        private readonly bool _isUseCharacterEffect;
        
        public EffectAction(BehaviourEffectData effectData, CharacterController controller) : base(null, controller)
        {
            _effectData = effectData;
            _getBattleEffect = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleEffect>(PoolableType.BattleEffect, _effectData.effectName);
        }
        
        public EffectAction(EffectBehaviourActionData actionData, CharacterController controller) : base(actionData, controller)
        {
            _effectData = actionData.effectData;
            _isUseCharacterEffect = actionData.isUseCharacterEffect;
            _getBattleEffect = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleEffect>(PoolableType.BattleEffect, _effectData.effectName);
        }

        
        public override void ActionExecute(bool isSkillBehaviour)
        {
            SetBattleEffect(isSkillBehaviour);
        }
    
        private void SetBattleEffect(bool isSkillBehaviour)
        {
            if (_isUseCharacterEffect)
            {
                var effect = _getBattleEffect.Invoke();
                SetEffectTransform(effect, Controller);
                BindEffectData(effect, Controller, _effectData.offsetType, isSkillBehaviour);
            }
            else
            {
                foreach (var target in GetTargetList.Invoke())
                {
                    var effect = _getBattleEffect.Invoke();
                    SetEffectTransform(effect, target);
                    BindEffectData(effect, target, _effectData.offsetType, isSkillBehaviour);
                }
            }
        }

        private void SetEffectTransform(BattleEffect effect, CharacterController controller)
        {
            switch (_effectData.offsetType)
            {
                case CharacterOffsetType.CharacterGround:
                    effect.transform.position = controller.transform.position;
                    break;
                case CharacterOffsetType.ProjectileOffset:
                    effect.transform.position = controller.offset.GetOffsetTransform(CharacterOffsetType.ProjectileOffset).position;
                    break;
                case CharacterOffsetType.HitOffset:
                    effect.transform.position = controller.offset.GetOffsetTransform(CharacterOffsetType.HitOffset).position;
                    break;
                case CharacterOffsetType.FluidHealthBarOffset:
                    effect.transform.position = controller.offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_effectData.canRotate)
            {
                effect.transform.rotation = controller.transform.rotation;
            }
        }

        private void BindEffectData(BattleEffect effect, CharacterController target, CharacterOffsetType offsetType, bool isSkillBehaviour)
        {
            switch (_effectData)
            {
                case OneShotEffect oneShotEffect:
                    effect.OneShotEffect();
                    if (isSkillBehaviour)
                    {
                        effect.SetSkillEffect();
                    }
                    break;
                case LoopEffect loopEffect:
                    effect.LoopEffect(loopEffect.duration);
                    effect.onBattleEvent.AddListener(() =>
                    {
                        effect.transform.position = target.offset.GetOffsetTransform(offsetType).position;
                    });
                    break;
            }
        }
    }
}
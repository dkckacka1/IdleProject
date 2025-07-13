using System;
using Cysharp.Threading.Tasks.Triggers;
using IdleProject.Battle.Effect;
using IdleProject.Core;
using IdleProject.Data.SkillData;
using UnityEngine;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class EffectAction : BehaviourAction
    {
        private readonly SkillEffectData _effectData;
        private readonly Func<BattleEffect> _getBattleEffect;

        private readonly bool _isUseCharacterEffect;
        
        public EffectAction(SkillEffectData effectData, CharacterController controller) : base(null, controller)
        {
            _effectData = effectData;
            _getBattleEffect = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleEffect>(PoolableType.BattleEffect, _effectData.effectName);
        }
        
        public EffectAction(EffectSkillActionData actionData, CharacterController controller) : base(actionData, controller)
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
            var effectTarget = _isUseCharacterEffect ? Controller : CurrentTarget; 
            if (effectTarget is null) return;
            
            var effect = _getBattleEffect.Invoke();
            switch (_effectData.offsetType)
            {
                case CharacterOffsetType.CharacterGround:
                    effect.transform.position = effectTarget.transform.position;
                    break;
                case CharacterOffsetType.ProjectileOffset:
                    effect.transform.position = effectTarget.offset.GetOffsetTransform(CharacterOffsetType.ProjectileOffset).position;
                    break;
                case CharacterOffsetType.HitOffset:
                    effect.transform.position = effectTarget.offset.GetOffsetTransform(CharacterOffsetType.HitOffset).position;
                    break;
                case CharacterOffsetType.FluidHealthBarOffset:
                    effect.transform.position = effectTarget.offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_effectData.canRotate)
            {
                effect.transform.rotation = Controller.transform.rotation;
            }
            
            BindEffectData(effect, effectTarget, _effectData.offsetType, isSkillBehaviour);
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
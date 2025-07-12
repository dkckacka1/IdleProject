using System;
using Cysharp.Threading.Tasks.Triggers;
using IdleProject.Battle.Effect;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class EffectAction : SkillAction
    {
        private readonly SkillEffectData _effectData;
        private readonly Func<BattleEffect> _getBattleEffect;
        
        public EffectAction(SkillEffectData effectData, CharacterController controller) : base(null, controller)
        {
            _effectData = effectData;
            _getBattleEffect = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleEffect>(PoolableType.BattleEffect, _effectData.effectName);
        }
        
        public EffectAction(EffectSkillActionData actionData, CharacterController controller) : base(actionData, controller)
        {
            _effectData = actionData.effectData;
            _getBattleEffect = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleEffect>(PoolableType.BattleEffect, _effectData.effectName);
        }

        
        public override void ActionExecute()
        {
            SetBattleEffect();
        }
    
        private void SetBattleEffect()
        {
            if (CurrentTarget is null) return;
            
            var effect = _getBattleEffect.Invoke();
            switch (_effectData.offsetType)
            {
                case CharacterOffsetType.CharacterGround:
                    effect.transform.position = CurrentTarget.transform.position;
                    break;
                case CharacterOffsetType.ProjectileOffset:
                    effect.transform.position = CurrentTarget.offset.GetOffsetTransform(CharacterOffsetType.ProjectileOffset).position;
                    break;
                case CharacterOffsetType.HitOffset:
                    effect.transform.position = CurrentTarget.offset.GetOffsetTransform(CharacterOffsetType.HitOffset).position;
                    break;
                case CharacterOffsetType.FluidHealthBarOffset:
                    effect.transform.position = CurrentTarget.offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_effectData.canRotate)
            {
                effect.transform.rotation = Controller.transform.rotation;
            }
        }
    }
}
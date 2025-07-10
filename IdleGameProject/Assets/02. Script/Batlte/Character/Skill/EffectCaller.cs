using System;
using System.Collections.Generic;
using IdleProject.Battle.Effect;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public class EffectCaller
    {
        private Func<BattleEffect> _getBattleEffect;
        private EffectCallOffsetType _callingOffsetType;

        public EffectCaller(Func<BattleEffect> getBattleEffect, EffectCallOffsetType callingOffsetType)
        {
            _getBattleEffect = getBattleEffect;
            _callingOffsetType = callingOffsetType;
        }

        public BattleEffect GetBattleEffect(CharacterController controller)
        {
            var effect =_getBattleEffect.Invoke();

            var offset = _callingOffsetType switch
            {
                EffectCallOffsetType.HitOffset => controller.HitEffectOffset,
                EffectCallOffsetType.Ground => controller,
                _ => throw new ArgumentOutOfRangeException()
            };

            effect.transform.position = offset;
            return effect;
        }
    }
}
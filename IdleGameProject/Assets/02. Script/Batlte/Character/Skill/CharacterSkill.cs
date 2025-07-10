using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character.Skill.SkillAction;
using IdleProject.Battle.Character.Skill.SkillRange;
using IdleProject.Battle.Character.Skill.SkillTarget;
using IdleProject.Battle.Effect;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkill
    {
        private readonly CharacterController _controller;
        private readonly ISkillRange _skillRange;
        private readonly ISkillAction _skillAction;
        private readonly ISkillGetTarget _getTarget;
        private readonly IEnumerator<EffectCaller> _directingEffects;
        
        public CharacterSkill(CharacterController controller, ISkillRange skillRange, ISkillAction skillAction, ISkillGetTarget getTarget, IEnumerable<EffectCaller> directingEffects)
        {
            _controller = controller;
            _skillRange = skillRange;
            _skillAction = skillAction;
            _getTarget = getTarget;
            _directingEffects = directingEffects?.GetEnumerator();
        }

        public void ExecuteSkill()
        {
            if (_directingEffects is not null)
            {
                _directingEffects.MoveNext();
                _directingEffects.Current.GetBattleEffect(_controller);
            }

            var targetList = _getTarget.GetTargetList(_controller).Where(_skillRange.GetInRange);

            _skillAction.ExecuteSkillAction(_controller, targetList);
        }
    }
}
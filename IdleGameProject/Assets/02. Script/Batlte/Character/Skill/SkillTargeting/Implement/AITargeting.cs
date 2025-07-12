using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class AITargeting : SkillTargeting
    {
        private readonly CharacterAIType _checkAIType;
        
        public AITargeting(CharacterController useController, AITargetingData aiTargetingData) : base(useController, aiTargetingData)
        {
            _checkAIType = aiTargetingData.targetAIType;
        }
        
        public override IEnumerable<CharacterController> TargetingCharacterList(List<CharacterController> compareTargetList,
            CharacterController currentTarget = null)
        {
            var targetAIType = _checkAIType == CharacterAIType.Ally ? GetCheckController(currentTarget).GetAiType :
                GetCheckController(currentTarget).GetAiType == CharacterAIType.Ally ? CharacterAIType.Enemy :
                CharacterAIType.Ally;

            return compareTargetList.Where(target => target.GetAiType == targetAIType);
        }
    }
}
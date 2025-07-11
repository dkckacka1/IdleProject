using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class AITargeting : SkillTargeting
    {
        private readonly CharacterAIType _checkAIType;
        
        public AITargeting(CharacterController useController, Data.StaticData.Skill.AITargetingData aiTargetingData) : base(useController, aiTargetingData)
        {
            _checkAIType = aiTargetingData.targetAIType;
        }
        
        public override bool TargetingCharacterList(CharacterController compareTarget, CharacterController currentTarget = null)
        {
            var targetAIType = _checkAIType == CharacterAIType.Ally ? CheckController(currentTarget).GetAiType :
                CheckController(currentTarget).GetAiType == CharacterAIType.Ally ? CharacterAIType.Enemy :
                CharacterAIType.Ally;

            return compareTarget.GetAiType == targetAIType;
        }
    }
}
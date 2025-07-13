using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class AITargeting : BehaviourTargeting
    {
        private readonly CharacterAIType _checkAIType;
        
        public AITargeting(CharacterController useController, AITargetingData aiTargetingData) : base(useController, aiTargetingData)
        {
            _checkAIType = aiTargetingData.targetAIType;
        }
        public override List<CharacterController> GetTargetingCharacterList(CharacterController targetCharacter, List<CharacterController> allCharacterList, List<CharacterController> currentTargetList)
        {
            var targetAIType = GetTargetAIType(UseSkillController);

            var checkList = GetCheckList(allCharacterList, currentTargetList);
            
            return checkList.Where(target => target.GetAiType == targetAIType).ToList();
        }
        
        private CharacterAIType GetTargetAIType(CharacterController controller)
        {
            return _checkAIType == CharacterAIType.Ally
                ? UseSkillController.characterAI.GetAllyType
                : UseSkillController.characterAI.GetEnemyType;
        }
    }
}
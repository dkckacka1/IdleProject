using System.Collections.Generic;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillTarget.Implement
{
    public class AllEnemy : ISkillGetTarget
    {
        public List<CharacterController> GetTargetList(CharacterController controller)
        {
            var enemyType = controller.GetAiType == CharacterAIType.Player
                ? CharacterAIType.Enemy
                : CharacterAIType.Player;
            
            return GameManager.GetCurrentSceneManager<BattleManager>().GetCharacterList(enemyType);
        }
    }
}

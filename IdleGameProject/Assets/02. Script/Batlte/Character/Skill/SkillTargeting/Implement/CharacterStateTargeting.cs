using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class CharacterStateTargeting : SkillTargeting
    {
        private readonly bool _isNot;
        private readonly CharacterStateTargetingData.CharacterStateTargetType _characterStateTargetType;
        
        public CharacterStateTargeting(CharacterController useSkillController, CharacterStateTargetingData targetingDataData) : base(useSkillController, targetingDataData)
        {
            _isNot = targetingDataData.isNot;
            _characterStateTargetType = targetingDataData.characterStateTargetType;
        }

        public override IEnumerable<CharacterController> TargetingCharacterList(List<CharacterController> compareTargetList, CharacterController currentTarget = null)
        {
            var checkController = GetCheckController(currentTarget);

            return _characterStateTargetType switch
            {
                CharacterStateTargetingData.CharacterStateTargetType.IsLive => GetIsLive(compareTargetList),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private IEnumerable<CharacterController> GetIsLive(List<CharacterController> compareTargetList)
        {
            return compareTargetList.Where(target => target.StatSystem.IsLive == !_isNot);
        }
    }
}
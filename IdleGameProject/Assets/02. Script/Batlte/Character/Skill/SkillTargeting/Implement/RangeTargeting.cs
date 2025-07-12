using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.SkillData;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillTargeting.Implement
{
    public class RangeTargeting : SkillTargeting
    {
        private readonly bool _isSelf;
        private readonly bool _hasSelf;
        private readonly bool _distinct;
        private readonly float _range;

        public RangeTargeting(CharacterController useSkillController, RangeTargetingData targetingDataData) : base(
            useSkillController, targetingDataData)
        {
            _isSelf = targetingDataData.isSelf;
            _hasSelf = targetingDataData.hasSelf;
            _distinct = targetingDataData.distinct;
            _range = targetingDataData.skillRange;
        }

        public override List<CharacterController> GetTargetingCharacterList(CharacterController userCharacter, List<CharacterController> allCharacterList, List<CharacterController> currentTargetList)
        {
            var checkList = GetCheckList(allCharacterList, currentTargetList);
            var targetList = new List<CharacterController>();
            if (_isSelf)
            {
                targetList.AddRange(checkList.Where(target => Vector3.Distance(target, userCharacter) < _range));
                if (_hasSelf)
                {
                    targetList.Add(userCharacter);
                }
            }
            else
            {
                foreach (var currentTarget in currentTargetList)
                {
                    targetList.AddRange(checkList.Where(target => Vector3.Distance(target, currentTarget) < _range));
                    if (_hasSelf)
                    {
                        targetList.Add(currentTarget);
                    }
                }                
            }

            if (_distinct)
            {
                targetList = targetList.Distinct().ToList();
            }
            
            return targetList;
        }
    }
}
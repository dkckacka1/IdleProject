using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Data.BehaviourData;
using UnityEngine;

namespace IdleProject.Battle.Character.Behaviour.Targeting.Implement
{
    public class CharacterStateTargeting : BehaviourTargeting
    {
        private readonly bool _isNot;
        private readonly CharacterStateTargetType _characterStateTargetType;

        public CharacterStateTargeting(CharacterController useSkillController,
            CharacterStateTargetingData targetingDataData) : base(useSkillController, targetingDataData)
        {
            _isNot = targetingDataData.isNot;
            _characterStateTargetType = targetingDataData.characterStateTargetType;
        }

        public override List<CharacterController> GetTargetingCharacterList(CharacterController targetCharacter,
            List<CharacterController> allCharacterList, List<CharacterController> currentTargetList)
        {
            var checkList = GetCheckList(allCharacterList, currentTargetList);

            return _characterStateTargetType switch
            {
                CharacterStateTargetType.IsLive => GetIsLive(checkList).ToList(),
                CharacterStateTargetType.IsInAttackRange => GetIsInAttackRange(checkList).ToList(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private IEnumerable<CharacterController> GetIsInAttackRange(List<CharacterController> compareTargetList)
        {
            return compareTargetList.Where(target =>
                Vector3.Distance(target, UseSkillController) <
                UseSkillController.StatSystem.GetStatValue(CharacterStatType.AttackRange));
        }

        private IEnumerable<CharacterController> GetIsLive(List<CharacterController> compareTargetList)
        {
            return compareTargetList.Where(target => target.StatSystem.IsLive == !_isNot);
        }
    }
}
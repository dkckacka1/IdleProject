using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Core.Time;
using IdleProject.Battle.AI;
using IdleProject.Battle.Character;
using Sirenix.OdinInspector;

namespace IdleProject.Battle
{
    public partial class BattleManager
    {
        private readonly Queue<CharacterController> _skillQueue = new Queue<CharacterController>();
        [ShowInInspector] private readonly List<Object> _currentSkillObjectList = new List<Object>();

        public bool IsAnyCharacterUsingSkill => _currentSkillObjectList.Count > 0; 
        
        public void AddSkillQueue(CharacterController useCharacter)
        {
            _skillQueue.Enqueue(useCharacter);
            AddSkillObject(useCharacter);
        }

        public void AddSkillObject(Object skillObject)
        {
            _currentSkillObjectList.Add(skillObject);
        }

        public void RemoveSkillObject(Object skillObject)
        {
            _currentSkillObjectList.Remove(skillObject);
            if (_currentSkillObjectList.Count <= 0)
            {
                ExitSkill(_skillQueue.Dequeue());
            }
        }

        private void UseSkill(CharacterController useCharacter)
        {
            useCharacter.isNowSkill = true;

            BattleStateEventBus.ChangeEvent(BattleStateType.Skill);
            TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, true);

            foreach (var character in GetCharacterList(CharacterAIType.Player)
                         .Where(character => useCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }

            foreach (var character in GetCharacterList(CharacterAIType.Enemy)
                         .Where(character => useCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }
        }

        public void ExitSkill(CharacterController useCharacter)
        {
            useCharacter.isNowSkill = false;
            useCharacter.StartAttackCooltime().Forget();

            BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
            TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, false);

            foreach (var character in GetCharacterList(CharacterAIType.Player))
            {
                character.AnimController.SetAnimationSpeed(GetCurrentBattleSpeed);
            }

            foreach (var character in GetCharacterList(CharacterAIType.Enemy))
            {
                character.AnimController.SetAnimationSpeed(GetCurrentBattleSpeed);
            }
        }
    }
}
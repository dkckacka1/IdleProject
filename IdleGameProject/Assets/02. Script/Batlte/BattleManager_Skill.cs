using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Core.Time;
using IdleProject.Character;
using IdleProject.Character.AI;
using Sirenix.OdinInspector;

namespace IdleProject.Battle
{
    public partial class BattleManager
    {
        private readonly Queue<BattleCharacterController> _skillQueue = new Queue<BattleCharacterController>();
        [ShowInInspector] private readonly List<Object> _currentSkillObjectList = new List<Object>();

        public bool IsAnyCharacterUsingSkill => _currentSkillObjectList.Count > 0; 
        
        public void AddSkillQueue(BattleCharacterController useBattleCharacter)
        {
            _skillQueue.Enqueue(useBattleCharacter);
            AddSkillObject(useBattleCharacter);
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

        private void UseSkill(BattleCharacterController useBattleCharacter)
        {
            useBattleCharacter.isNowSkill = true;

            BattleStateEventBus.ChangeEvent(BattleStateType.Skill);
            TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, true);

            foreach (var character in GetCharacterList(CharacterAIType.Player)
                         .Where(character => useBattleCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }

            foreach (var character in GetCharacterList(CharacterAIType.Enemy)
                         .Where(character => useBattleCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }
        }

        public void ExitSkill(BattleCharacterController useBattleCharacter)
        {
            useBattleCharacter.isNowSkill = false;
            useBattleCharacter.StartAttackCooltime().Forget();

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
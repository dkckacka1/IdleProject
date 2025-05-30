
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Events;
using Engine.Core.EventBus;
using Engine.Core.Time;
using Engine.Util.Extension;
using Engine.Util;
using IdleProject.Core.UI;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle
{
    public enum BattleStateType
    {
        Ready = default,
        Battle,
        Skill,
        Win,
        Defeat,
    }

    public enum GameStateType
    {
        Play = default,
        Pause
    }
    
    public enum BattleObjectType
    {
        Character,
        Projectile,
        Effect,
        UI
    }

    public partial class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        [HideInInspector] public SpawnController spawnController;
        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly EnumEventBus<GameStateType> GameStateEventBus = new();
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new();

        public Transform effectParent;
        public Transform projectileParent;

        private readonly Queue<CharacterController> _skillQueue = new Queue<CharacterController>(); 

        public List<CharacterController> GetCharacterList(CharacterAIType aiType) => (aiType == CharacterAIType.Player) ? playerCharacterList : enemyCharacterList;

        protected override void Initialized()
        {
            base.Initialized();
            spawnController = GetComponent<SpawnController>();
            UIManager.Instance.GetUIController<BattleUIController>().Initialized();
            EnumExtension.Foreach<BattleObjectType>((type) =>
            {
                BattleObjectEventDic.Add(type, new UnityEvent());
            });
        }

        private void FixedUpdate()
        {
            if (GameStateEventBus.CurrentType is GameStateType.Play)
            {
                switch (BattleStateEventBus.CurrentType)
                {
                    case BattleStateType.Ready:
                        break;
                    case BattleStateType.Battle:
                        if (_skillQueue.Count > 0)
                        {
                            UseSkill(_skillQueue.Dequeue());
                        }
                        BattleObjectEventDic[BattleObjectType.Character].Invoke();
                        BattleObjectEventDic[BattleObjectType.Projectile].Invoke();
                        BattleObjectEventDic[BattleObjectType.Effect].Invoke();
                        break;
                    case BattleStateType.Skill:
                        BattleObjectEventDic[BattleObjectType.Projectile].Invoke();
                        BattleObjectEventDic[BattleObjectType.Effect].Invoke();
                        break;
                    case BattleStateType.Win:
                        break;
                    case BattleStateType.Defeat:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void LateUpdate()
        {
            if (GameStateEventBus.CurrentType is GameStateType.Play && BattleStateEventBus.CurrentType is BattleStateType.Battle)
            {
                BattleObjectEventDic[BattleObjectType.UI].Invoke();
            }
        }

        public void AddCharacterController(CharacterController controller)
        {
            var characterControllerList = GetCharacterList(controller.characterAI.aiType);

            characterControllerList.Add(controller);
        }

        public void DeathCharacter(CharacterController characterController)
        {
            var aiType = characterController.characterAI.aiType;
            var characterList = GetCharacterList(aiType);

            if (characterList.Any(character => character.StatSystem.IsLive) is false)
            {
                if (aiType == CharacterAIType.Player)
                {
                    BattleStateEventBus.ChangeEvent(BattleStateType.Defeat);
                }
                else
                {
                    BattleStateEventBus.ChangeEvent(BattleStateType.Win);
                }
            }
        }
        
        public void AddSkillQueue(CharacterController useCharacter)
        {
            _skillQueue.Enqueue(useCharacter);
        }

        private void UseSkill(CharacterController useCharacter)
        {
            BattleStateEventBus.ChangeEvent(BattleStateType.Skill);
            TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, true);
            
            foreach (var character in GetCharacterList(CharacterAIType.Player).Where(character => useCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }
            
            foreach (var character in GetCharacterList(CharacterAIType.Enemy).Where(character => useCharacter != character))
            {
                character.AnimController.SetAnimationSpeed(0f);
            }
        }

        public void ExitSkill()
        {
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


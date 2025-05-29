using UnityEngine;

using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using Engine.Core.Time;
using Engine.Util.Extension;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using UnityEngine.Events;
using IdleProject.Core.UI;
using IdleProject.Battle.UI;
using UnityEngine.InputSystem.Switch;

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

    public enum BattleSpeedType
    {
        Default = 1,
        Double = 2,
        Threefold = 3
    }
    
    public enum BattleObjectType
    {
        Character,
        Projectile,
        Effect,
        UI
    }

    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        [HideInInspector] public SpawnController spawnController;

        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly EnumEventBus<GameStateType> GameStateEventBus = new();
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new();

        public Transform effectParent;
        public Transform projectileParent;

        #region 전투 배속 관련

        public const string BattleSpeedTimeKey = "BattleSpeed";
        public static float GetCurrentBattleSpeed => TimeManager.Instance.GetTimeScaleFactor(BattleSpeedTimeKey);
        public static float GetCurrentBattleDeltaTime => TimeManager.Instance.GetDeltaTimeScale(BattleSpeedTimeKey);
        public static UnityEvent<float> GetChangeBattleSpeedEvent =>
            TimeManager.Instance.GetFactorChangeEvent(BattleSpeedTimeKey);
        public static UniTask GetBattleTimer(float waitTime) =>
            TimeManager.Instance.StartTimer(waitTime, BattleSpeedTimeKey);

        #endregion

        private readonly Queue<CharacterController> _skillQueue = new Queue<CharacterController>(); 

        private List<CharacterController> GetCharacterList(CharacterAIType aiType) => (aiType == CharacterAIType.Playerable) ? playerCharacterList : enemyCharacterList;

        public override void Initialized()
        {
            base.Initialized();
            spawnController = GetComponent<SpawnController>();
            UIManager.Instance.GetUIController<BattleUIController>().initialized();
            EnumExtension.Foreach<BattleObjectType>((type) =>
            {
                BattleObjectEventDic.Add(type, new());
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

        public IEnumerable<CharacterController> GetCharacterList(CharacterAIType aiType, Func<CharacterController, bool> whereFunc = null)
        {
            IEnumerable<CharacterController> result = GetCharacterList(aiType);

            if (whereFunc is not null)
            {
                result = result.Where(whereFunc);
            }

            return result;
        }

        public void DeathCharacter(CharacterController characterController)
        {
            var aiType = characterController.characterAI.aiType;
            var characterList = GetCharacterList(aiType);

            if (characterList.Any(character => character.StatSystem.isLive) is false)
            {
                if (aiType == CharacterAIType.Playerable)
                {
                    Defeat();
                }
                else
                {
                    Win();
                }
            }
        }

        public void SetBattleSpeed(BattleSpeedType battleSpeedType)
        {
            TimeManager.Instance.SetTimeScaleFactor(BattleSpeedTimeKey, (float)battleSpeedType);
        }

        private void Win()
        {
            BattleStateEventBus.ChangeEvent(BattleStateType.Win);
        }

        private void Defeat()
        {
            BattleStateEventBus.ChangeEvent(BattleStateType.Defeat);
        }

        public void AddSkillQueue(CharacterController useCharacter)
        {
            _skillQueue.Enqueue(useCharacter);
        }
        
        public void UseSkill(CharacterController useCharacter)
        {
            BattleStateEventBus.ChangeEvent(BattleStateType.Skill);
            TimeManager.Instance.SettingTimer(BattleSpeedTimeKey, true);
            
            foreach (var character in GetCharacterList(CharacterAIType.Playerable))
            {
                if (useCharacter != character)
                {
                    character.AnimController.SetAnimationSpeed(0f);
                }
            }
            
            foreach (var character in GetCharacterList(CharacterAIType.Enemy))
            {
                if (useCharacter != character)
                {
                    character.AnimController.SetAnimationSpeed(0f);
                }
            }
        }

        public void ExitSkill()
        {
            BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
            TimeManager.Instance.SettingTimer(BattleSpeedTimeKey, false);
            
            foreach (var character in GetCharacterList(CharacterAIType.Playerable))
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


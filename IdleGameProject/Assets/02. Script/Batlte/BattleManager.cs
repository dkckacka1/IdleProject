using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Engine.Core.EventBus;
using Engine.Util.Extension;
using IdleProject.Core.UI;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using IdleProject.Character;
using IdleProject.Character.AI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;

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

    public partial class BattleManager : SceneController
    {
        [HideInInspector] public SpawnController spawnController;
        [HideInInspector] public List<BattleCharacterController> playerCharacterList = new List<BattleCharacterController>();
        [HideInInspector] public List<BattleCharacterController> enemyCharacterList = new List<BattleCharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly EnumEventBus<GameStateType> GameStateEventBus = new();
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new();

        public Transform effectParent;
        public Transform projectileParent;

        private BattleUIController _battleUIController;

        private const string BATTLE_INIT_TASK = "BattleInit";
        
        public List<BattleCharacterController> GetCharacterList(CharacterAIType aiType) =>
            aiType == CharacterAIType.Player ? playerCharacterList : enemyCharacterList;
        
        public override async UniTask Initialize()
        {
            spawnController = GetComponent<SpawnController>();
            _battleUIController = UIManager.Instance.GetUIController<BattleUIController>();
            
            EnumExtension.Foreach<BattleObjectType>(type => { BattleObjectEventDic.Add(type, new UnityEvent()); });
            
            TaskChecker.StartLoading(BATTLE_INIT_TASK, _battleUIController.Initialized);
            TaskChecker.StartLoading(BATTLE_INIT_TASK, SpawnCharacter);
            
            await UniTask.WaitUntil(() => TaskChecker.IsTasking(BATTLE_INIT_TASK) is false);
        }

        private async void Start()
        {
            await UniTask.WaitUntil(() => UIManager.Instance.IsShowingLoading is false);
            _battleUIController.PlayReadyUI(() =>
            {
                GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.ChangeEvent(BattleStateType.Battle);
                GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Play);
            });
        }

        private async UniTask SpawnCharacter()
        {
            await spawnController.SpawnCharacterAtInfo(CharacterAIType.Player,
                DataManager.Instance.DataController.playerFormationInfo);
            await spawnController.SpawnCharacterAtInfo(CharacterAIType.Enemy,
                DataManager.Instance.DataController.enemyFormationInfo);
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
                            UseSkill(_skillQueue.Peek());
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
            if (GameStateEventBus.IsSameCurrentType(GameStateType.Play) &&
                BattleStateEventBus.IsSameCurrentType(BattleStateType.Battle, BattleStateType.Skill))
            {
                BattleObjectEventDic[BattleObjectType.UI].Invoke();
            }
        }

        public void AddCharacterController(BattleCharacterController controller)
        {
            var characterControllerList = GetCharacterList(controller.characterAI.aiType);

            characterControllerList.Add(controller);
        }

        public void DeathCharacter(BattleCharacterController battleCharacterController)
        {
            var aiType = battleCharacterController.characterAI.aiType;
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

    }
}
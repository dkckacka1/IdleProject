using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using Engine.Core.Time;
using Engine.Util.Extension;
using IdleProject.Battle.Spawn;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Sound;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.Events;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle
{
    public partial class BattleManager : SceneController, IEnumEvent<BattleStateType>
    {
        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly Dictionary<BattleObjectType, UnityEvent> SkillObjectEventDic = new();
        public readonly EnumEventBus<BattleGameStateType> GameStateEventBus = new(BattleGameStateType.Play);
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new(BattleStateType.Ready);

        public Transform effectParent;
        public Transform projectileParent;

        [HideInInspector] public SpawnController spawnController;

        public List<CharacterController> GetCharacterList() => new List<CharacterController>()
            .Concat(playerCharacterList).Concat(enemyCharacterList).ToList();
        
        public List<CharacterController> GetCharacterList(CharacterAIType aiType) =>
            aiType == CharacterAIType.Ally ? playerCharacterList : enemyCharacterList;

        private const string BATTLE_INIT_TASK = "BattleInit";
        
        public override async UniTask Initialize()
        {
            spawnController = GetComponent<SpawnController>();
            spawnController.Initialize();
            
            BattleStateEventBus.PublishEvent(this);
            
            EnumExtension.Foreach<BattleObjectType>(type =>
            {
                BattleObjectEventDic.Add(type, new UnityEvent());
                SkillObjectEventDic.Add(type, new UnityEvent());
            });
            
            TaskChecker.StartLoading(BATTLE_INIT_TASK, SpawnCharacter);
            await TaskChecker.WaitTasking(BATTLE_INIT_TASK);
        }

        private async UniTask SpawnCharacter()
        {
            await spawnController.SpawnCharacterByFormation(CharacterAIType.Ally,
                DataManager.Instance.DataController.Player.GetFormation());
            await spawnController.SpawnCharacterByFormation(CharacterAIType.Enemy,
                DataManager.Instance.DataController.selectStaticStageData.stageFormation);
        }

        private void Start()
        {
            SoundManager.Instance.PlayBGM(DataManager.Instance.ConstData.BattleSceneBgmName);
        }

        private void FixedUpdate()
        {
            if (GameStateEventBus.CurrentType is BattleGameStateType.Play)
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
                        SkillObjectEventDic[BattleObjectType.Projectile].Invoke();
                        SkillObjectEventDic[BattleObjectType.Effect].Invoke();
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
            if (GameStateEventBus.IsSameCurrentType(BattleGameStateType.Play) &&
                BattleStateEventBus.CurrentType != BattleStateType.Ready)
            {
                BattleObjectEventDic[BattleObjectType.UI].Invoke();
            }
        }

        public void AddCharacterController(CharacterController controller)
        {
            var characterControllerList = GetCharacterList(controller.characterAI.aiType);

            characterControllerList.Add(controller);
        }

        public void DeathCharacter()
        {
            if (BattleStateEventBus.IsSameCurrentType(BattleStateType.Defeat, BattleStateType.Win))
                return;
            
            if (BattleStateEventBus.IsSameCurrentType(BattleStateType.Skill) is false)
            {
                SetBattleResultState();
            }
        }

        private void SetBattleResultState()
        {
            var characterList = GetCharacterList(CharacterAIType.Ally);
            if (characterList.Any(character => character.StatSystem.IsLive) is false)
                    BattleStateEventBus.ChangeEvent(BattleStateType.Defeat);
            
            characterList = GetCharacterList(CharacterAIType.Enemy);
            if (characterList.Any(character => character.StatSystem.IsLive) is false)
                BattleStateEventBus.ChangeEvent(BattleStateType.Win);
            
        }

        public void OnEnumChange(BattleStateType type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                    break;
                case BattleStateType.Battle:
                    TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, false);
                    break;
                case BattleStateType.Skill:
                    TimeManager.Instance.SettingTimer(BATTLE_SPEED_TIME_KEY, true);
                    break;
                case BattleStateType.Win:
                    Win();
                    SoundManager.Instance.PauseBGM();
                    SoundManager.Instance.PlaySfx("SFX_Win");
                    break;
                case BattleStateType.Defeat:
                    SoundManager.Instance.PauseBGM();
                    SoundManager.Instance.PlaySfx("SFX_Defeat");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void Win()
        {
            var currentStage = DataManager.Instance.DataController.selectStaticStageData;
            DataManager.Instance.DataController.Player.ClearStage(currentStage);
            foreach (var reward in currentStage.rewardDataList)
            {
                switch (reward.itemData)
                {
                    case StaticConsumableItemData consumableItem:
                        DataManager.Instance.DataController.Player.AddConsumableItem(consumableItem.Index, reward.count);
                        break;
                    case StaticEquipmentItemData equipmentItem:
                        DataManager.Instance.DataController.Player.AddEquipmentItem(equipmentItem.Index);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
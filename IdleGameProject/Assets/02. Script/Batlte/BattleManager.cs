using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Engine.Core.EventBus;
using Engine.Util.Extension;
using IdleProject.Battle.Spawn;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Sound;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle
{
    public partial class BattleManager : SceneController, IEnumEvent<BattleStateType>
    {
        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly EnumEventBus<BattleGameStateType> GameStateEventBus = new(BattleGameStateType.Play);
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new(BattleStateType.Ready);

        public Transform effectParent;
        public Transform projectileParent;

        [HideInInspector] public SpawnController spawnController;

        public List<CharacterController> GetCharacterList(CharacterAIType aiType) =>
            aiType == CharacterAIType.Player ? playerCharacterList : enemyCharacterList;

        private const string BATTLE_INIT_TASK = "BattleInit";
        
        public override async UniTask Initialize()
        {
            spawnController = GetComponent<SpawnController>();
            spawnController.Initialize();
            
            BattleStateEventBus.PublishEvent(this);
            
            EnumExtension.Foreach<BattleObjectType>(type => { BattleObjectEventDic.Add(type, new UnityEvent()); });
            TaskChecker.StartLoading(BATTLE_INIT_TASK, SpawnCharacter);
            await TaskChecker.WaitTasking(BATTLE_INIT_TASK);
        }

        private async UniTask SpawnCharacter()
        {
            await spawnController.SpawnCharacterByFormation(CharacterAIType.Player,
                DataManager.Instance.DataController.Player.PlayerFormation);
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
            if (BattleStateEventBus.IsSameCurrentType(BattleStateType.Skill) is false)
            {
                SetBattleResultState();
            }
        }

        private void SetBattleResultState()
        {
            var characterList = GetCharacterList(CharacterAIType.Player);
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
                case BattleStateType.Battle:
                case BattleStateType.Skill:
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
            foreach (var reward in currentStage.rewardList)
            {
                switch (reward.rewardType)
                {
                    case RewardType.ConsumableItem:
                        DataManager.Instance.DataController.Player.AddConsumableItem(reward.itemIndex, reward.count);
                        break;
                    case RewardType.EquipmentItem:
                        DataManager.Instance.DataController.Player.AddEquipmentItem(reward.itemIndex);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
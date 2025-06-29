using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Engine.Core.EventBus;
using Engine.Core.Time;
using Engine.Util.Extension;
using IdleProject.Core.UI;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using IdleProject.Battle.UI;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using UnityEngine.Serialization;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using Object = UnityEngine.Object;

namespace IdleProject.Battle
{


    public partial class BattleManager : SceneController
    {
        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public readonly Dictionary<BattleObjectType, UnityEvent> BattleObjectEventDic = new();
        public readonly EnumEventBus<BattleGameStateType> GameStateEventBus = new(BattleGameStateType.Play);
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new(BattleStateType.Ready);

        public Transform effectParent;
        public Transform projectileParent;

        [HideInInspector] public SpawnController spawnController;
        private BattleUIController _battleUIController;

        private const string BATTLE_INIT_TASK = "BattleInit";
        
        public List<CharacterController> GetCharacterList(CharacterAIType aiType) =>
            aiType == CharacterAIType.Player ? playerCharacterList : enemyCharacterList;
        
        public override async UniTask Initialize()
        {
            _battleUIController = UIManager.Instance.GetUIController<BattleUIController>();
            
            spawnController = GetComponent<SpawnController>();
            spawnController.Initialize();
            
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

            // TODO : 승리 스테이지 플레이더 데이터에 저장
            // 전투 보상 설정
            characterList = GetCharacterList(CharacterAIType.Enemy);
            if (characterList.Any(character => character.StatSystem.IsLive) is false)
                BattleStateEventBus.ChangeEvent(BattleStateType.Win);
            
        }
    }
}
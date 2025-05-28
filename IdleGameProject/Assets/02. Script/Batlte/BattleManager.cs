using UnityEngine;

using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using Engine.Core.Time;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using IdleProject.Battle.AI;
using IdleProject.Battle.Spawn;
using UnityEngine.Events;
using IdleProject.Core.UI;
using IdleProject.Battle.UI;

namespace IdleProject.Battle
{
    public enum BattleStateType
    {
        Ready,
        Battle,
        Win,
        Defeat,
    }

    public enum GameStateType
    {
        Play,
        Pause
    }

    public enum BattleSpeedType
    {
        Default = 1,
        Double = 2,
        Threefold = 3
    }

    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        [HideInInspector] public SpawnController spawnController;

        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        [HideInInspector] public UnityEvent battleEvent = new UnityEvent();
        [HideInInspector] public UnityEvent battleUIEvent = new UnityEvent();

        public readonly EnumEventBus<GameStateType> GameStateEventBus = new();
        public readonly EnumEventBus<BattleStateType> BattleStateEventBus = new();

        public Transform effectParent;
        public Transform projectileParent;

        public const string BattleSpeedTimeKey = "BattleSpeed";

        public static float GetCurrentBattleSpeed => TimeManager.Instance.GetTimeScaleFactor(BattleSpeedTimeKey);
        public static float GetCurrentBattleDeltaTime => TimeManager.Instance.GetDeltaTimeScale(BattleSpeedTimeKey);
        public static UnityEvent<float> GetChangeBattleSpeedEvent =>
            TimeManager.Instance.GetFactorChangeEvent(BattleSpeedTimeKey);
        public static UniTask GetBattleTimer(float waitTime) =>
            TimeManager.Instance.StartTimer(waitTime, BattleSpeedTimeKey);

        private List<CharacterController> GetCharacterList(CharacterAIType aiType) => (aiType == CharacterAIType.Playerable) ? playerCharacterList : enemyCharacterList;

        public override void Initialized()
        {
            base.Initialized();
            spawnController = GetComponent<SpawnController>();
            UIManager.Instance.GetUIController<BattleUIController>().initialized();
        }

        private void FixedUpdate()
        {
            if (GameStateEventBus.CurrentType is GameStateType.Play && BattleStateEventBus.CurrentType is BattleStateType.Battle)
            {
                battleEvent?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (GameStateEventBus.CurrentType is GameStateType.Play && BattleStateEventBus.CurrentType is BattleStateType.Battle)
            {
                battleUIEvent?.Invoke();
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
            characterList.Remove(characterController);

            if (characterList.Count <= 0)
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
        
    }
}


using UnityEngine;

using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using System;

using Engine.Core.EventBus;

using CharacterController = IdleProject.Battle.Character.CharacterController;
using IdleProject.Battle.AI;
using Sirenix.OdinInspector;
using IdleProject.Battle.Spawn;
using UnityEngine.Events;

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



    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        [HideInInspector] public SpawnController spawnController;

        [HideInInspector] public List<CharacterController> playerCharacterList = new List<CharacterController>();
        [HideInInspector] public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        [HideInInspector] public UnityEvent battleEvent = new UnityEvent();
        [HideInInspector] public UnityEvent battleUIEvent = new UnityEvent();

        public EnumEventBus<GameStateType> gameStateEventBus = new();
        public EnumEventBus<BattleStateType> battleStateEventBus = new();

        public Transform effectParent;
        public Transform projectileParent;

        private List<CharacterController> GetCharacterList(CharacterAIType aiType) => (aiType == CharacterAIType.Playerable) ? playerCharacterList : enemyCharacterList;

        public override void Initialized()
        {
            base.Initialized();
            spawnController = GetComponent<SpawnController>();
        }

        private void FixedUpdate()
        {
            if (gameStateEventBus.CurrentType is GameStateType.Play && battleStateEventBus.CurrentType is BattleStateType.Battle)
            {
                battleEvent?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (gameStateEventBus.CurrentType is GameStateType.Play)
            {
                battleUIEvent?.Invoke();
            }
        }

        public void AddCharacterController(CharacterController controller, CharacterAIType aiType)
        {
            var characterControllerList = GetCharacterList(aiType);

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

        public void DeathCharacter(CharacterController characterController, CharacterAIType aiType)
        {
            var characterList = GetCharacterList(aiType);
            characterList.Remove(characterController);

            if (characterList.Count <= 0)
            {
                if(aiType == CharacterAIType.Playerable)
                {
                    Defeat();
                }
                else
                {
                    Win();
                }
            }
        }


        private void Win()
        {
            battleStateEventBus.ChangeEvent(BattleStateType.Win);
        }

        private void Defeat()
        {
            battleStateEventBus.ChangeEvent(BattleStateType.Defeat);
        }

        public void Test()
        {
            foreach (var c in playerCharacterList)
            {
                Destroy(c.gameObject    );
            }

            foreach (var c in enemyCharacterList)
            {
                Destroy(c.gameObject);
            }

            playerCharacterList.Clear();
            enemyCharacterList.Clear();
            battleEvent.RemoveAllListeners();
            battleUIEvent.RemoveAllListeners();
            gameStateEventBus = new();
            battleStateEventBus = new();
        }
    }
}


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

namespace IdleProject.Battle
{
    public enum BattleStateType
    {
        Win,
        Defeat,
        Battle,
    }

    public enum GameStateType
    {
        Play,
        Pause
    }

    public enum CharacterAIType
    {
        Playerable,
        Enemy,
        //Ally,
        //Neutral,
    }

    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        public SpawnController spawnController;

        public List<CharacterController> playerCharacterList = new List<CharacterController>();
        public List<CharacterController> enemyCharacterList = new List<CharacterController>();

        public BattleStateType battleStateType;
        public GameStateType gameStateType;

        public EventBusController<GameStateType> gameStateEventBus;
        public EventBusController<BattleStateType> battleStateEventBus;

        public override void Initialized()
        {
            base.Initialized();
            spawnController = GetComponent<SpawnController>();
        }


        private void FixedUpdate()
        {
            if (gameStateType is GameStateType.Play)
            {
            }
        }

        public void AddCharacterController(CharacterController controller, CharacterAIType aiType)
        {
            var characterControllerList = (aiType == CharacterAIType.Playerable) ? playerCharacterList : enemyCharacterList;

            characterControllerList.Add(controller);
        }

        public IEnumerable<CharacterController> GetCharacterList(CharacterAIType aiType, Func<CharacterController, bool> whereFunc = null)
        {
            IEnumerable<CharacterController> result = ChooseCharacterList(aiType);

            if (whereFunc is not null)
            {
                result = result.Where(whereFunc);
            }

            return result;
        }

        private IEnumerable<CharacterController> ChooseCharacterList(CharacterAIType aiType)
        {
            IEnumerable<CharacterController> result = null;

            switch (aiType)
            {
                case CharacterAIType.Playerable:
                    result = playerCharacterList;
                    break;
                case CharacterAIType.Enemy:
                    result = enemyCharacterList;
                    break;
            }

            return result;
        }
    }
}

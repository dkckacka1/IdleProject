using UnityEngine;

using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using System;

using CharacterController = IdleProject.Battle.Character.CharacterController;
using IdleProject.Battle.AI;
using Sirenix.OdinInspector;

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

    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        public List<CharacterController> playerCharacterList = new List<CharacterController>();
        public List<CharacterController> enemyCharacterList = new List<CharacterController>();
        public List<BattleAIAble> battleAIList = new List<BattleAIAble>();

        public BattleStateType battleStateType;
        public GameStateType gameStateType;

        private void FixedUpdate()
        {
            if (gameStateType is GameStateType.Play)
            {
                foreach (var AI in battleAIList)
                {
                    AI.UpdateAI();
                }
            }
        }

        public IEnumerable<CharacterController> GetCharacterList(bool isEnemy, Func<CharacterController, bool> whereFunc = null)
        {
            IEnumerable<CharacterController> result = isEnemy ? enemyCharacterList : playerCharacterList;

            if (whereFunc is not null)
            {
                result = result.Where(whereFunc);
            }

            return result;
        }

        [Button]
        public void TestPlay()
        {
            foreach (var battleAI in battleAIList)
            {
                battleAI.PlayAI();
            }
        }
    }
}

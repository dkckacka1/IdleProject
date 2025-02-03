using UnityEngine;

using Engine.Util;
using System.Collections.Generic;
using System.Linq;
using System;

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

        public BattleStateType battleStateType;
        public GameStateType gameStateType;



        private void Update()
        {
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
    }
}

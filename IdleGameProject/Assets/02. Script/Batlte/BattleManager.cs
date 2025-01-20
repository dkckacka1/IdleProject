using UnityEngine;

using Engine.Util;
using System.Collections.Generic;

namespace IdleProject.Battle
{
    public enum BattleStateType
    {
        Win,
        Defeat,
        Battle,
    }

    public class BattleManager : SingletonMonoBehaviour<BattleManager>
    {
        public List<CharacterController> playerCharacterList = new List<CharacterController>();
        public List<CharacterController> enemyCharacterList = new List<CharacterController>();
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public int PlayerLevel = 1;
        public int PlayerExp = 0;
        
        public List<PlayerHeroData> UserHeroList;

        public FormationInfo UserFormation;
    }
}
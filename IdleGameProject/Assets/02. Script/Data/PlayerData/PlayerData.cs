using System.Collections.Generic;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public PlayerInfo PlayerInfo;
        
        public List<PlayerCharacterData> playerCharacterList;
        public List<PlayerConsumableItemData> playerConsumableItemList;
        public List<PlayerEquipmentItemData> playerEquipmentItemList;
        public List<string> playerClearStageList;

        public string frontMiddleCharacterName;
        public string frontRightCharacterName;
        public string frontLeftCharacterName;
        public string rearRightCharacterName;
        public string rearLeftCharacterName;
    }
}
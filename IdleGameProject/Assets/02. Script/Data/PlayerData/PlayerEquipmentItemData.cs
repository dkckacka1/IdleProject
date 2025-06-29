﻿using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;
using UnityEngine.Serialization;

namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerEquipmentItemData
    {
        public int index;
        public string itemIndex;
        public string equipmentCharacterName;

        public StaticEquipmentItemData GetData => DataManager.Instance.GetData<StaticEquipmentItemData>(itemIndex);
    }
}
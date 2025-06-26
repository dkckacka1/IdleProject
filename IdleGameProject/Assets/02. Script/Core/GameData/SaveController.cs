using System.Linq;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;

namespace IdleProject.Core.GameData
{
    public class SaveController
    {
        private readonly PlayerData _playerData;

        public SaveController()
        {
        }
        
        public SaveController(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void SaveAll(DynamicPlayerData playerData)
        {
            foreach (var character in playerData.PlayerCharacterDataDic.Values)
            {
                SaveCharacter(character);
            }

            foreach (var consumableItem in playerData.PlayerConsumableItemDataDic.Values)
            {
                SaveConsumableItem(consumableItem);
            }
        }

        public void SaveCharacter(DynamicCharacterData characterData)
        {
            var playerCharacter = _playerData.playerCharacterList.FirstOrDefault(character =>
                character.characterName == characterData.StaticData.Index);

            if (playerCharacter is null)
                // 캐릭터 추가
            {
                var newCharacter = new PlayerCharacterData
                {
                    characterName = characterData.StaticData.name,
                    level = characterData.Level,
                    exp = characterData.Exp,
                };
                _playerData.playerCharacterList.Add(newCharacter);
            }
            else
            {
                playerCharacter.level = characterData.Level;
                playerCharacter.exp = characterData.Exp;
            }
        }

        public void SaveConsumableItem(DynamicConsumableItemData itemData)
        {
            var playerItem = _playerData.playerConsumableItemList.FirstOrDefault(item =>
                item.itemName == itemData.StaticData.Index);
            
            if (playerItem is null)
                // 캐릭터 추가
            {
                var newItem = new PlayerConsumableItemData
                {
                    itemName = itemData.StaticData.Index,
                    itemCount = itemData.itemCount
                };
                _playerData.playerConsumableItemList.Add(newItem);
            }
            else
            {
                playerItem.itemCount = itemData.itemCount;
            }
        }
    }
}
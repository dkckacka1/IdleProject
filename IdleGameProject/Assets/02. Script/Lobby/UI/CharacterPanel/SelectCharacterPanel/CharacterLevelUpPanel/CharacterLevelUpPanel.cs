using System.Linq;
using Cysharp.Threading.Tasks;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.StaticData;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterLevelUpPanel : UIPanel
    {
        [SerializeField] private Transform slotContent;

        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CharacterLevelUpButton").Button.onClick.AddListener(LevelUp);

            var expItemDataList = DataManager.Instance.GetDataList<ConsumableItemData>()
                .Where(data => data.consumableType == ConsumableType.CharacterExp);

            foreach (var itemData in expItemDataList)
            {
                var slot = CreateSlot(itemData);
                slot.SetCount(DataManager.Instance.DataController.userData.GetItem(itemData.Index).itemCount, true);
            }
        }

        private ConsumableItemSlot CreateSlot(ConsumableItemData itemData)
        {
            var slot = SlotUI.GetSlotUI<ConsumableItemSlot>(slotContent);
            slot.SetData(itemData);
            return slot.GetComponent<ConsumableItemSlot>();
        }

        private void LevelUp()
        {
            Debug.Log("LevelUp");
        }
    }
}
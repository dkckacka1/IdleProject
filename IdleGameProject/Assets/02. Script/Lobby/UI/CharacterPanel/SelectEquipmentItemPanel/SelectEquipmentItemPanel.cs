using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class SelectEquipmentItemPanel : UIPanel, IUISelectEquipmentItemUpdatable
    {
        
        public override void Initialized()
        {
        }
        
        public void SelectEquipmentItem(StaticEquipmentItemData item)
        {
            OpenPanel();
        }
    }
}

using IdleProject.Data.DynamicData;
using TMPro;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    public class CharacterSlot : SlotParts
    {
        [SerializeField] private TextMeshProUGUI levelText;

        public void SetLevel(int level)
        {
            levelText.text = $"Lv {level}";
        }
        
        public override void ShowParts<T>(T data)
        {
            base.ShowParts(data);

            if (data is DynamicCharacterData character)
            {
                SetLevel(character.Level);
            }
        }
    }
}

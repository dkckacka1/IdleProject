using UnityEngine;

namespace IdleProject.Core.UI
{
    public class PickCharacterPanelSlot : SelectSlot
    {
        [SerializeField] private GameObject choiceObject;

        public void SetChoice(bool isActive)
        {
            choiceObject.SetActive(isActive);
        }
    }
}

using UnityEngine;

namespace IdleProject.Core.UI
{
    public class PickCharacterPopupSlot : SelectSlot
    {
        [SerializeField] private GameObject choiceObject;

        public void SetChoice(bool isActive)
        {
            choiceObject.SetActive(isActive);
        }
    }
}

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace IdleProject.Lobby.UI
{
    public class ToastUIPopup : UIPopup
    {
        [BoxGroup("ToastPopup"), SerializeField] protected TextMeshProUGUI descriptionText;
        
        public void SetToast(string description)
        {
            descriptionText.text = description;
        }
    }
}
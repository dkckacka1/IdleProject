using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class ToastPopup : StaticPopupUI
    {
        [BoxGroup("ToastPopup"), SerializeField] protected TextMeshProUGUI descriptionText;
        
        public void SetToast(string description)
        {
            descriptionText.text = description;
        }
    }
}
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class ConfirmUIPopup : UIPopup
    {
        [BoxGroup("ConfirmPopup"), SerializeField] private Button yesButton;
        [BoxGroup("ConfirmPopup"), SerializeField] protected TextMeshProUGUI descriptionText;

        private UnityAction _yesAction;

        protected override void Awake()
        {
            base.Awake();
            yesButton.onClick.AddListener(() => _yesAction?.Invoke());
            yesButton.onClick.AddListener(ClosePopup);
        }

        public void SetConfirm(string description, UnityAction yesAction)
        {
            descriptionText.text = description;
            _yesAction = yesAction;
        }

        public override void ClosePopup()
        {
            base.ClosePopup();
            _yesAction = null;
        }
    }
}
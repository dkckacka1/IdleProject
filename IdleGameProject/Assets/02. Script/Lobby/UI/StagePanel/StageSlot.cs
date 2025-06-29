using System;
using IdleProject.Core.UI;
using IdleProject.Data.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.StagePanel
{
    [RequireComponent(typeof(Button))]
    public class StageSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private Image clearTypoImage;

        [HideInInspector] public Button button;

        private StaticStageData _stageData;
        
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void SetStage(StaticStageData stageData, bool isClear)
        {
            stageText.text = $"{stageData.chapterIndex}-{stageData.stageIndex}";
            clearTypoImage.enabled = isClear;

            _stageData = stageData;
        }

        private void OnClick()
        {
            if (_stageData is null) return;

            Debug.Log(_stageData.Index);
            
            foreach (var updatable in UIManager.Instance.GetUIsOfType<IUISelectStageUpdatable>())
            {
                updatable.SelectStageUpdatable(_stageData);
            }
        }
    }
}

using System;
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
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void SetStage(StaticStageData stageData, bool isClear)
        {
            stageText.text = $"{stageData.chapterIndex}-{stageData.stageIndex}";
            clearTypoImage.enabled = isClear;
        }
    }
}

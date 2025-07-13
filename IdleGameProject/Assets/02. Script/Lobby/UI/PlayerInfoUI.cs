using IdleProject.Core;
using IdleProject.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class PlayerInfoUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerLevelText;
        [SerializeField] private Slider playerExpSlider;
        [SerializeField] private TextMeshProUGUI playerExpValueText;

        public void SetPlayerInfo(PlayerInfo playerInfo)
        {
            playerNameText.text = playerInfo.playerName;
            playerLevelText.text = playerInfo.playerLevel.ToString();

            var levelUpExpValue = playerInfo.GetMaxExp();
            playerExpSlider.maxValue = levelUpExpValue;
            playerExpSlider.value = playerInfo.playerExp;
            playerExpValueText.text = $"{playerInfo.playerExp}/{levelUpExpValue}";
        }
    }
}
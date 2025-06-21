using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data;
using UnityEngine;

namespace IdleProject.Lobby.UI
{
    public class LobbyUIController : UIController
    {
        public async UniTask Initialized()
        {
            UIManager.Instance.GetUI<UIPopup>("CharacterPopup").Initialized();
            UIManager.Instance.GetUI<UIPopup>("CharacterPopup").ClosePopup();
            UIManager.Instance.GetUI<UIButton>("EquipmentButton").Button.onClick.AddListener(OpenEquipmentPanel);
            UIManager.Instance.GetUI<UIButton>("DungeonButton").Button.onClick.AddListener(GoToDungeon);
        }

        private void GoToDungeon()
        {
            DataManager.Instance.DataController.selectStageData = DataManager.Instance.GetData<StageData>("1");
            GameManager.Instance.LoadScene(SceneType.Battle);
        }

        private void OpenEquipmentPanel()
        {
            UIManager.Instance.GetUI<UIPopup>("CharacterPopup").OpenPopup();
        }
    }
}

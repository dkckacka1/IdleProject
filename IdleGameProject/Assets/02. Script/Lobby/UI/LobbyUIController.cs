using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data.StaticData;
using IdleProject.Lobby.UI.CharacterPanel;
using UnityEngine;

namespace IdleProject.Lobby.UI
{
    public class LobbyUIController : UIController
    {
        public async UniTask Initialized()
        {
            UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>().ClosePanel();
            UIManager.Instance.GetUI<StagePanel.StagePanel>().ClosePanel();
            UIManager.Instance.GetUI<UIButton>("EquipmentButton").Button.onClick.AddListener(OpenEquipmentPanel);
            UIManager.Instance.GetUI<UIButton>("DungeonButton").Button.onClick.AddListener(GoToDungeon);
        }

        private void GoToDungeon()
        {
            // UIManager.Instance.GetUI<StagePanel.StagePanel>().OpenPanel();
            
            DataManager.Instance.DataController.selectStaticStageData = DataManager.Instance.GetData<StaticStageData>("1");
            GameManager.Instance.LoadScene(SceneType.Battle);
        }

        private void OpenEquipmentPanel()
        {
            UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>().OpenPanel();
        }
    }
}

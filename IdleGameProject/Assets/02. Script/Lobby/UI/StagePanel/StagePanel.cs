using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Lobby.UI.StagePanel
{
    public class StagePanel : UIPanelCanvas
    {
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseStagePanelButton").Button.onClick.AddListener(ClosePanel);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            
            UIManager.Instance.GetUI<ShowStagePanel>().OpenPanel();
        }
    }
}

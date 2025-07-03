using System.Collections.Generic;
using System.Linq;

namespace IdleProject.Core.UI
{
    public class PanelRadioGroup
    {
        private readonly List<UIPanel> _radioPanelList = new();

        public void PublishPanel(UIPanel panel)
        {
            _radioPanelList.Add(panel);
            panel.RadioGroup = this;
        }

        public void UnPublishPanel(UIPanel panel)
        {
            _radioPanelList.Remove(panel);
            panel.RadioGroup = null;
        }

        public void OpenNotify(UIPanel notifyPanel)
        {
            foreach (var panel in _radioPanelList.Where(panel => panel != notifyPanel))
            {
                if(panel.IsOpened)
                    panel.ClosePanel();
            }
        }
    }
}
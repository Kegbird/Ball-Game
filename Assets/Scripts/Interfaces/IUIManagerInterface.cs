using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUIManagerInterface
    {
        public void ShowBlackScreen();
        public void HideBlackScreen();
        public void ShowEndText(string text);
        public void HideObjectiveText();
        public void UpdateTimerText(float time);
        public void HideTimerText();
        public void ShowTimerText(float time);
    }
}

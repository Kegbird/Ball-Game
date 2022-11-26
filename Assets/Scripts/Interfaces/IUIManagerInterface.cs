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
        public void ShowVictoryText();
        public void HideObjectiveText();
    }
}

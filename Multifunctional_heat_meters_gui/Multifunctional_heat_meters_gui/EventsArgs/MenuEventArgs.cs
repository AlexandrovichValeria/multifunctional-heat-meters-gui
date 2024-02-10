using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.EventsArgs
{
    public class MenuEventArgs : EventArgs
    {
        public List<int> ButtonsNumbers { get; set; }
        public View.ContentMenu.DeepButtonsNames ButtonName { get; set; }
        public MenuEventArgs(View.ContentMenu.DeepButtonsNames buttonName, List<int> formsNumbers)
        {
            ButtonsNumbers = formsNumbers;
            ButtonName = buttonName;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Multifunctional_heat_meters_gui.EventsArgs
{
    public class NextFormArgs : EventArgs
    {
        public Dictionary<string, string> Params { get; set; }

        public NextFormArgs(Dictionary<string, string> paramsToNextForm)
        {
            Params = paramsToNextForm;
        }
    }
}


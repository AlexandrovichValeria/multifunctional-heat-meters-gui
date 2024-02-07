using System;
using System.Collections.Generic;

namespace Setup_database_for_device.EventsArgs
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

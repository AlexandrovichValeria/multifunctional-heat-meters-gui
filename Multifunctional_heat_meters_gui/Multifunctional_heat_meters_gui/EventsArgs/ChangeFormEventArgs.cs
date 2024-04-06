using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.EventsArgs
{
    public class ChangeFormEventArgs : EventArgs
    {
        public string Data { get; set; }
        public int PipelineIndex { get; set; }
        public ChangeFormEventArgs(string type, int pipelineIndex)
        {
            Data = type;
            PipelineIndex = pipelineIndex;
        }
    }
}

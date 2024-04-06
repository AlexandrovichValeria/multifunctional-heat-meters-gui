using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.EventsArgs
{
    public class SensorTypeEventArgs : EventArgs
    {
        public string SensorType { get; set; }
        public int PipelineIndex { get; set; }
        public SensorTypeEventArgs(string type, int pipelineIndex)
        {
            SensorType = type;
            PipelineIndex = pipelineIndex;
        }
    }
}

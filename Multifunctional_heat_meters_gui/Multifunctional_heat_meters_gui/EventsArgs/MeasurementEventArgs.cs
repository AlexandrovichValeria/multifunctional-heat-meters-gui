using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.EventsArgs
{
    public class MeasurementEventArgs : EventArgs
    {
        public int typeOfMeasurement { get; set; }
        public MeasurementEventArgs(int type)
        {
            typeOfMeasurement = type;
        }
    }
}

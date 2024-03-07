using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class PressureSensor : Sensor
    {
        public PressureSensor(bool active = true)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _parameters.Add("113н01", new Parameter("113н01", "03201", "", ""));
            _parameters.Add("032н00", new Parameter("032н00", "042", "", ""));
            _parameters.Add("032н01", new Parameter("032н01", "16,31", "кгс/см2", "pressure"));
            _parameters.Add("032н02", new Parameter("032н02", "0", "", ""));
            _parameters.Add("032н08", new Parameter("032н08", "0", "кгс/см2", "pressure"));
        }
    }
}

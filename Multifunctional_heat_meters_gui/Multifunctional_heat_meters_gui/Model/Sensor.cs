using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Sensor
    {
        protected bool _active;

        protected Dictionary<string, Parameter> _parameters;

        protected int _type;

        protected int _channel_number;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Dictionary<string, Parameter> Parameters
        {
            get { return _parameters; }
        }

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int ChannelNumber
        {
            get { return _channel_number; }
            set { _channel_number = value; }
        }

        public Sensor(bool active = false)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _channel_number = 0;
        }
        public void ChangeParameterValue(string parameterName, string value)
        {
            _parameters[parameterName].Value = value;
        }

        public virtual void ChangePowerMeasurement(int unitOfMeasurement){ }
        
        public virtual void ChangePressureMeasurement(int unitOfMeasurement) { }
    }
}

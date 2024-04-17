using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Sensor : AbstractModel
    {
        protected bool _active;

        //protected Dictionary<string, Parameter> _parameters;

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

        public Sensor(int type, bool active = false)
        {
            _active = active;
            _channel_number = 0;
            _parameters = new Dictionary<string, Parameter>();
            _type = type;
            if (_type == 1 || _type == 4)
            {
                _parameters.Add("033н00", new Parameter("033н00", "043", "", ""));
                _parameters.Add("033н01", new Parameter("033н01", "180", "'C", "temperature"));
                _parameters.Add("033н02", new Parameter("033н02", "0", "'C", "temperature"));
            }
            else
            {
                _parameters.Add("032н00", new Parameter("032н00", "042", "", ""));
                _parameters.Add("032н01", new Parameter("032н01", "16.31", "кгс/см2", "pressure"));
                _parameters.Add("032н02", new Parameter("032н02", "0", "", ""));
                _parameters.Add("032н08", new Parameter("032н08", "0", "кгс/см2", "pressure"));
            }
        }
    }
}

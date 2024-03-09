using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class Sensor : WindowForm
    {
        private Builder _builder;
        protected string _type;
        protected Sensor(string type, Builder builder, IntPtr handle) : base($"Датчик {type}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            //_formName = formName;
            //_backForwardComponent = BackForwardComponent.Create();

            SetupHandlers();
        }

        public virtual Dictionary<string, string> GetSensorSettings()
        {
            return new Dictionary<string, string>() { };
        }

        public virtual void ChangePressureMeasurement(int unitOfMeasurement) { }
        public virtual void ChangePowerMeasurement(int unitOfMeasurement) { }

        protected void SetupHandlers()
        {
            
        }
    }
}

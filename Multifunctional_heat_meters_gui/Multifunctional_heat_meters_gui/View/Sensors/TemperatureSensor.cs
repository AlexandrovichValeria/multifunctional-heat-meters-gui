using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class TemperatureSensor : Sensor
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private Label name_label;

        //private string _type;
        public static TemperatureSensor Create(string type)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.Sensors.TemperatureSensor.glade", null);
            return new TemperatureSensor(type, builder, builder.GetObject("form_box").Handle);
        }

        protected TemperatureSensor(string type, Builder builder, IntPtr handle) : base($"температуры {type}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _type = type;
            name_label.Text = _formName;
            button_box.Add(_backForwardComponent);
        }
    }
}

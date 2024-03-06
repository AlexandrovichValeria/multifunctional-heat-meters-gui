using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class PressureSensor : Sensor
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private Label name_label;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;

        public static PressureSensor Create(string type)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.Sensors.PressureSensor.glade", null);
            return new PressureSensor(type, builder, builder.GetObject("form_box").Handle);
        }

        protected PressureSensor(string type, Builder builder, IntPtr handle) : base($"давления {type}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _type = type;
            name_label.Text = _formName;
            button_box.Add(_backForwardComponent);
        }

        public override Dictionary<string, string> GetSensorSettings()
        {
            string[] cv1 = { "040", "041", "042" };

            string combo1Value = ""; //"042" по умолчанию
            if (combo1.ActiveId != "-1")
                combo1Value = cv1[Int32.Parse(combo1.ActiveId)];
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                {"032н00", combo1Value},
                {"032н01", entry1.Text},
                {"032н02", "0"},
                {"032н08", entry2.Text},
            };
            return result;
        }
    }
}

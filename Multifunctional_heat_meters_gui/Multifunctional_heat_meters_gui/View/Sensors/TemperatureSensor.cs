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
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;

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

        public override Dictionary<string, string> GetSensorSettings()
        {
            string[] cv1 = { "023", "024", "033", "034", "043", "044", "053", "054", "063", "064" };
            string combo1Value = ""; //"043" по умолчанию
            if (combo1.ActiveId != "-1")
                combo1Value = cv1[Int32.Parse(combo1.ActiveId)];

            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                {"033н00", combo1Value},
                {"033н01", entry2.Text},
                {"033н02", entry1.Text}
            };
            return result;
        }
    }
}

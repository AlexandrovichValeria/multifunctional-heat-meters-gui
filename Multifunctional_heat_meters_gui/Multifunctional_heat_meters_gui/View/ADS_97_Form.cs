using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class ADS_97_Form : Dialog
    {
        private Builder _builder;

        [Builder.Object]
        private Button button1;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private Label label4;
        [Builder.Object]
        private SpinButton spinbutton1;
        [Builder.Object]
        private SpinButton spinbutton2;

        public static ADS_97_Form Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.ADS_97_Form.glade", null);
            return new ADS_97_Form(builder, builder.GetObject("dialog").Handle);
        }

        protected ADS_97_Form(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            label4.Hide();
            spinbutton2.Hide();

            SetupHandlers();
        }

        public Dictionary<string, string> GetADS_97_results()
        {
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                { "038н00", combo1.ActiveId },
                { "038н01", spinbutton1.Value.ToString() },
            };
            if (combo1.ActiveId == "1")
                result.Add("038н02", spinbutton2.Value.ToString());

            return result;
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            button1.Clicked += OnButton1Click;
            combo1.Changed += OnComboChanged;
        }
        protected void OnButton1Click(object sender, EventArgs a)
        {
            Hide();
        }
        protected void OnComboChanged(object sender, EventArgs a)
        {
            if (combo1.ActiveId == "0")
            {
                label4.Hide();
                spinbutton2.Hide();
            }
            else
            {
                label4.Show();
                spinbutton2.Show();
            }
        }
    }
}

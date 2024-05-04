using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using System.Text.RegularExpressions;

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

        public event EventHandler ADSChanged;
        private int ADSAmount;

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
            ADSAmount = 0;

            SetupHandlers();
        }

        public Dictionary<string, string> GetADS_97_results()
        {
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                { "038н00", ADSAmount.ToString()},
                { "038н01", spinbutton1.Value.ToString() },
            };
            if (combo1.ActiveId == "1")
                result.Add("038н02", spinbutton2.Value.ToString());

            return result;
        }

        public void ChangeAdaptersAmount(int amount)
        {
            if(amount == 1)
            {
                combo1.ActiveId = "0";
            }
            else if (amount == 2)
            {
                combo1.ActiveId = "1";
            }
        }

        public int GetADSAmount()
        {
            return Int32.Parse(combo1.ActiveId) + 1;
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            spinbutton1.Changed += TurnIntoNumber;
            spinbutton2.Changed += TurnIntoNumber;

            button1.Clicked += OnButton1Click;
            combo1.Changed += OnComboChanged;
        }

        protected void OnButton1Click(object sender, EventArgs a)
        {
            ADSAmount = combo1.Active + 1;
            ADSChanged?.Invoke(this, EventArgs.Empty);
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

        protected void TurnIntoNumber(object sender, EventArgs e)
        {
            SpinButton temp = (SpinButton)sender;

            string text = temp.Text;
            string numberOnly = Regex.Replace(text, "[^0-9. ,-]", "");
            numberOnly = numberOnly.Replace(",", ".");
            temp.Text = numberOnly;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class CoolantSelectionForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private Box gas_box;
        [Builder.Object]
        private Box liquid_box;

        private GasBlock gas_block;
        private LiquidBlock liquid_block;

        public static CoolantSelectionForm Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.CoolantSelectionForm.glade", null);
            return new CoolantSelectionForm(index, builder, builder.GetObject("form_box").Handle);
        }

        protected CoolantSelectionForm(int index, Builder builder, IntPtr handle) : base($"Теплоноситель {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            button_box.Add(_backForwardComponent);

            gas_block = GasBlock.Create();
            gas_box.Add(gas_block);
            gas_box.Hide();

            liquid_block = LiquidBlock.Create();
            liquid_box.Add(liquid_block);
            liquid_box.Hide();

            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            combo1.Changed += Combo1ChangedEvent;
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }
        protected void Combo1ChangedEvent(object sender, EventArgs a)
        {
            switch (combo1.ActiveId)
            {
                case "0":
                    gas_box.Hide();
                    liquid_box.Hide();
                    break;
                case "1":
                    gas_box.Show();
                    liquid_box.Hide();
                    break;
                case "2":
                    gas_box.Show();
                    liquid_box.Hide();
                    break;
                case "3":
                    gas_box.Hide();
                    liquid_box.Show();
                    break;
            }
        }
    }
}

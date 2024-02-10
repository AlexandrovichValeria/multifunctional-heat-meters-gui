using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class ADS_97_Form : Window
    {
        private Builder _builder;
        public static ADS_97_Form Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.ADS_97_Form.glade", null);
            return new ADS_97_Form(builder, builder.GetObject("window1").Handle);
        }

        protected ADS_97_Form(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }
    }
}

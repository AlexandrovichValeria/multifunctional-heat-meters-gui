using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class BackForwardButtons : Box
    {
        private Builder _builder;

        public static BackForwardButtons Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.BackForwardButtons.glade", null);
            return new BackForwardButtons(builder, builder.GetObject("button_box").Handle);
        }

        protected BackForwardButtons(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }
        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
        }
    }
}

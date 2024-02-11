using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class GasBlock : Frame
    {
        private Builder _builder;
        public static GasBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.GasBlock.glade", null);
            return new GasBlock(builder, builder.GetObject("frame").Handle);
        }
        protected GasBlock(Builder builder, IntPtr handle) : base(handle)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class LiquidBlock : Frame
    {
        private Builder _builder;
        public static LiquidBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.LiquidBlock.glade", null);
            return new LiquidBlock(builder, builder.GetObject("frame").Handle);
        }
        protected LiquidBlock(Builder builder, IntPtr handle) : base(handle)
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

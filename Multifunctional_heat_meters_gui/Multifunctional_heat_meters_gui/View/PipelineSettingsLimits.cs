using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class PipelineSettingsLimits : WindowForm
    {
        
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        
        public static PipelineSettingsLimits Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.PipelineSettingsLimits.glade", null);
            return new PipelineSettingsLimits(index, builder, builder.GetObject("form_box").Handle);
        }

        protected PipelineSettingsLimits(int index, Builder builder, IntPtr handle) : base($"Первая настройка трубопровода {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            button_box.Add(_backForwardComponent);
            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            
        }

    }
}

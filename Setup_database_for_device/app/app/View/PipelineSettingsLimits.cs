using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    class PipelineSettingsLimits : WindowForm
    {
        
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        
        public static PipelineSettingsLimits Create(int index)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.PipelineSettingsLimits.glade", null);
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

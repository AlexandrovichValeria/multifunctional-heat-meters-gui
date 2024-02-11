using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class PipelineSettings1Form : WindowForm
    {
        
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        
        public static PipelineSettings1Form Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.PipelineSettings1Form.glade", null);
            return new PipelineSettings1Form(index, builder, builder.GetObject("form_box").Handle);
        }

        protected PipelineSettings1Form(int index, Builder builder, IntPtr handle) : base($"Первая настройка трубопровода {index}", builder, handle)
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

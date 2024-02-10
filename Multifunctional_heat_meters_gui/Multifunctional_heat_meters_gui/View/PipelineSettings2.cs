using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class PipelineSettings2 : WindowForm
    {

        private Builder _builder;
        [Builder.Object]
        private Box button_box;

        public static PipelineSettings2 Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.PipelineSettings2.glade", null);
            return new PipelineSettings2(index, builder, builder.GetObject("form_box").Handle);
        }

        protected PipelineSettings2(int index, Builder builder, IntPtr handle) : base($"Вторая настройка трубопровода {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            button_box.Add(_backForwardComponent);
            SetupHandlers();
        }
        
        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }
    }
}

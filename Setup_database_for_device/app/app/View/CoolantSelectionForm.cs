using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    public class CoolantSelectionForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;

        public static CoolantSelectionForm Create(int index)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.CoolantSelectionForm.glade", null);
            return new CoolantSelectionForm(index, builder, builder.GetObject("form_box").Handle);
        }

        protected CoolantSelectionForm(int index, Builder builder, IntPtr handle) : base($"Теплоноситель {index}", builder, handle)
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

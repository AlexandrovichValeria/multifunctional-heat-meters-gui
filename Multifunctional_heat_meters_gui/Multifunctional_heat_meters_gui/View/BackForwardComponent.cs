using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class BackForwardComponent : Box
    {
        private Builder _builder;

        [Builder.Object]
        private Button back_button;
        [Builder.Object]
        private Button forward_button;
        [Builder.Object]
        private Button save_button;
        //[Builder.Object]
        //private CheckButton value_check_button;

        public event EventHandler BackButtonClickedEvent;
        public event EventHandler ForwardButtonClickedEvent;
        public event EventHandler SaveButtonClickedEvent;

        public static BackForwardComponent Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.BackForwardComponent.glade", null);
            return new BackForwardComponent(builder, builder.GetObject("box").Handle);
        }

        protected BackForwardComponent(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }
        public void SetForwardButtonSensitive()
        {
            forward_button.Sensitive = true;
        }
        public void SetForwardButtonInsensitive()
        {
            forward_button.Sensitive = false;
        }

        protected void SetupHandlers()
        {
            back_button.Clicked += BackButton_Click;
            forward_button.Clicked += ForwardButton_Click;
            save_button.Clicked += SaveButton_Click;

            //value_check_button.Clicked += ValueCheckButtonClick;
            //DeleteEvent += OnLocalDeleteEvent;
        }
        protected void BackButton_Click(object sender, EventArgs e)
        {
            BackButtonClickedEvent?.Invoke(this, e);
        }

        protected void ForwardButton_Click(object sender, EventArgs e)
        {
            ForwardButtonClickedEvent?.Invoke(this, e);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveButtonClickedEvent?.Invoke(this, e);
        }
    }
}

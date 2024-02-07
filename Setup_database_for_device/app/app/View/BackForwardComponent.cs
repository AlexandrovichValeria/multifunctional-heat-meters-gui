using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    public class BackForwardComponent : Box
    {
        private Builder _builder;

        [Builder.Object]
        private Button back_button;
        [Builder.Object]
        private Button forward_button;

        public event EventHandler BackButtonClickedEvent;
        public event EventHandler ForwardButtonClickedEvent;

        public static BackForwardComponent Create()
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.BackForwardComponent.glade", null);
            return new BackForwardComponent(builder, builder.GetObject("box").Handle);
        }

        protected BackForwardComponent(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }
        protected void SetupHandlers()
        {
            back_button.Clicked += BackButton_Click;
            forward_button.Clicked += ForwardButton_Click;
            //DeleteEvent += OnLocalDeleteEvent;
        }
        protected void BackButton_Click(object sender, EventArgs e)
        {
            BackButtonClickedEvent?.Invoke(this, e);
        }

        protected void ForwardButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("ForwardButton_Click");
            ForwardButtonClickedEvent?.Invoke(this, e);
        }
    }
}

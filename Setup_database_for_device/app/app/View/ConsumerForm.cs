using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    class ConsumerForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;

        [Builder.Object]
        private ComboBox combo1;

        public static ConsumerForm Create(List<int> pipelinesNumbers, int consumerNumber)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.ConsumerForm.glade", null);
            return new ConsumerForm(pipelinesNumbers, consumerNumber, builder, builder.GetObject("form_box").Handle);
        }

        protected ConsumerForm(List<int> pipelinesNumbers, int consumerNumber, Builder builder, IntPtr handle) : base($"п{consumerNumber}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _formIndex = consumerNumber;

            Image image = new Image("10.jpg");
            button_box.Add(_backForwardComponent);

            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            combo1.Changed += OnComboChanged;
        }

        protected void OnComboChanged(object sender, EventArgs a)
        {
            Console.WriteLine("AAAAAAAAA");
            Console.WriteLine(combo1.ActiveId);
        }

        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }


    }
}

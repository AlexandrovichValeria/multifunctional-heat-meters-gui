using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    class TestForm : WindowForm
    {
        private Builder _builder;
        public static TestForm Create(List<int> pipelinesNumbers, int consumerNumber)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.TestForm.glade", null);
            return new TestForm(pipelinesNumbers, consumerNumber, builder, builder.GetObject("grid1").Handle);
        }

        protected TestForm(List<int> pipelinesNumbers, int consumerNumber, Builder builder, IntPtr handle) : base($"п{consumerNumber}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }
    }
}

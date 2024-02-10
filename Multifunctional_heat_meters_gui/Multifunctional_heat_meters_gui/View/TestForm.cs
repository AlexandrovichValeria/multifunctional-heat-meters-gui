using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class TestForm : WindowForm
    {
        private Builder _builder;
        public static TestForm Create(List<int> pipelinesNumbers, int consumerNumber)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.TestForm.glade", null);
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

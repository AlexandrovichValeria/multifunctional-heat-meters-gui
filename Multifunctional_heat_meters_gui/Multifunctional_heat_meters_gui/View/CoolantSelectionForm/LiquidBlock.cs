using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class LiquidBlock : WindowBlock
    {
        private Builder _builder;

        [Builder.Object]
        private Entry entry0;
        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;
        [Builder.Object]
        private Entry entry3;
        [Builder.Object]
        private Entry entry4;
        [Builder.Object]
        private Entry entry5;
        [Builder.Object]
        private Entry entry6;
        [Builder.Object]
        private Entry entry7;

        public static LiquidBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.LiquidBlock.glade", null);
            return new LiquidBlock(builder, builder.GetObject("box").Handle);
        }
        protected LiquidBlock(Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            parameter_widget = new Dictionary<string, Entry>
            {
                { "125н00", entry0 },
                { "125н01", entry1 },
                { "125н02", entry2 },
                { "125н03", entry3 },
                { "125н04", entry4 },
                { "125н05", entry5 },
                { "125н06", entry6 },
                { "125н07", entry7 },
            };

            SetupHandlers();

        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "125н00", parameter_widget["125н00"].Text },
                { "125н01", parameter_widget["125н01"].Text },
                { "125н02", parameter_widget["125н02"].Text },
                { "125н03", parameter_widget["125н03"].Text },
                { "125н04", parameter_widget["125н04"].Text },
                { "125н05", parameter_widget["125н05"].Text },
                { "125н06", parameter_widget["125н06"].Text },
                { "125н07", parameter_widget["125н07"].Text },
            };
            return res;
        }

        protected void SetupHandlers()
        {
            foreach (KeyValuePair<string, Entry> keyval in parameter_widget)
            {
                parameter_widget[keyval.Key].Changed += TurnIntoNumber;
                parameter_widget[keyval.Key].Changed += (sender, e) => OnValueChanged(sender, new List<string> { keyval.Key, keyval.Value.Text });
            }
        }
    }
}

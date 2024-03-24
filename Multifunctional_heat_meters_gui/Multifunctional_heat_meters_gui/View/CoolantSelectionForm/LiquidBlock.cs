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
            SetupHandlers();

        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "125н00", entry0.Text },
                { "125н01", entry1.Text },
                { "125н02", entry2.Text },
                { "125н03", entry3.Text },
                { "125н04", entry4.Text },
                { "125н05", entry5.Text },
                { "125н06", entry6.Text },
                { "125н07", entry7.Text },
            };
            return res;
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            entry0.Changed += OnBlockChanged;
            entry1.Changed += OnBlockChanged;
            entry2.Changed += OnBlockChanged;
            entry3.Changed += OnBlockChanged;
            entry4.Changed += OnBlockChanged;
            entry5.Changed += OnBlockChanged;
            entry6.Changed += OnBlockChanged;
            entry7.Changed += OnBlockChanged;

            entry0.Changed += TurnIntoNumber;
            entry1.Changed += TurnIntoNumber;
            entry2.Changed += TurnIntoNumber;
            entry3.Changed += TurnIntoNumber;
            entry4.Changed += TurnIntoNumber;
            entry5.Changed += TurnIntoNumber;
            entry6.Changed += TurnIntoNumber;
            entry7.Changed += TurnIntoNumber;
        }
    }
}

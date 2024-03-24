using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class GasBlock : WindowBlock
    {
        private Builder _builder;

        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;


        public static GasBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.GasBlock.glade", null);
            return new GasBlock(builder, builder.GetObject("box").Handle);
        }
        protected GasBlock(Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
            
        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "104", entry1.Text }, //ширина зоны насыщения
                { "105", entry2.Text }, //степень сухости
            };
            return res;
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            entry1.Changed += OnBlockChanged;
            entry2.Changed += OnBlockChanged;

            entry1.Changed += TurnIntoNumber;
            entry2.Changed += TurnIntoNumber;
        }
    }
}

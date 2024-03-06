using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using System.Text.RegularExpressions;

namespace Multifunctional_heat_meters_gui.View
{
    public class WindowBlock : Box
    {
        private Builder _builder;

        public event EventHandler BlockChangedEvent;
        protected WindowBlock(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            ShowAll();
            SetupHandlers();
        }

        public virtual Dictionary<string, string> GetResult()
        {
            return new Dictionary<string, string>();
        }

        public virtual void SetData(Dictionary<string, string> data)
        {
            
        }

        public virtual void EnableBlock()
        {
            Sensitive = true;
        }

        public virtual void DisableBlock()
        {
            Sensitive = false;
        }

        protected void SetupHandlers()
        {

        }
        protected void TurnIntoNumber(object sender, EventArgs e)
        {
            Entry temp = (Entry)sender;

            string text = temp.Text;
            string numberOnly = Regex.Replace(text, "[^0-9. ,-]", "");
            temp.Text = numberOnly;
        }

        protected void OnBlockChanged(object sender, EventArgs e)
        {
            BlockChangedEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}

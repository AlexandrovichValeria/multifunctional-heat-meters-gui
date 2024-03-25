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
        public bool blockFilledCorrectly;

        public event EventHandler BlockChangedEvent;

        protected WindowBlock(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            ShowAll();
            //SetupHandlers();
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

        /*protected bool CheckParameterValidation(string param_name, string param_value)
        {
            //get info from a table and send comparisson result
            if(!Dictionaries.parameterPatterns.ContainsKey(param_name))
                return true;
            if (Regex.IsMatch(param_value, Dictionaries.parameterPatterns[param_name]))
                return true;
            return false;
        }*/

        /*protected void SetupHandlers()
        {

        }*/

        protected void TurnIntoNumber(object sender, EventArgs e)
        {
            Entry temp = (Entry)sender;

            string text = temp.Text;
            string numberOnly = Regex.Replace(text, "[^0-9. ,-]", "");
            numberOnly = numberOnly.Replace(",", ".");
            temp.Text = numberOnly;
        }

        protected void OnValueChanged(object sender, List<string> e)
        {
            string param_name = e[0];
            string param_value = e[1];
            if(!ParameterIsValid(param_name, param_value)) {
                //show error
                ShowErrorMessage(param_name);
                blockFilledCorrectly = false;
                //block going forward and saving the file
            }
            else
            {
                //remove error
                HideErrorMessage(param_name);
                //change blockFilledCorrectly
            }
            OnBlockChanged(sender, EventArgs.Empty);
        }
        protected virtual void ShowErrorMessage(string param_name)
        {

        }

        protected virtual void HideErrorMessage(string param_name)
        {

        }


        protected bool ParameterIsValid(string param_name, string param_value)
        {
            //get info from a table and send comparisson result
            if (!Dictionaries.parameterPatterns.ContainsKey(param_name))
                return true;
            if (Regex.IsMatch(param_value, Dictionaries.parameterPatterns[param_name]))
                return true;
            return false;
        }

        protected void OnBlockChanged(object sender, EventArgs e)
        {
            BlockChangedEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}

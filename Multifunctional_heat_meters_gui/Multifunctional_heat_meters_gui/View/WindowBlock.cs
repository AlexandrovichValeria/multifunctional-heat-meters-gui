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
        //public bool blockFilledCorrectly;

        protected bool AutoCheckFlag;
        protected Dictionary<string, Entry> parameter_widget;

        public event EventHandler BlockChangedEvent;

        protected WindowBlock(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            ShowAll();
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
            if (AutoCheckFlag == true) {
                string param_name = e[0];
                string param_value = e[1];
                if (!ParameterIsValid(param_name, param_value))
                    ShowErrorMessage(param_name);
                else
                    HideErrorMessage(param_name);
            }
            OnBlockChanged(sender, EventArgs.Empty);
        }

        protected virtual void ShowErrorMessage(string param_name)
        {
            if (parameter_widget.ContainsKey(param_name))
            {
                parameter_widget[param_name].StyleContext.AddClass("incorrect-value");
            }
        }

        protected virtual void HideErrorMessage(string param_name)
        {
            if (parameter_widget.ContainsKey(param_name))
            {
                parameter_widget[param_name].StyleContext.RemoveClass("incorrect-value");
            }
        }

        public void SetAutoValueCheck(bool flag)
        {
            AutoCheckFlag = flag;

            if (!(parameter_widget is null))
            {
                if (AutoCheckFlag)
                {
                    foreach (KeyValuePair<string, Entry> keyval in parameter_widget)
                    {
                        if (!ParameterIsValid(keyval.Key, keyval.Value.Text))
                            ShowErrorMessage(keyval.Key);
                        else
                            HideErrorMessage(keyval.Key);
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, Entry> keyval in parameter_widget)
                    {
                        HideErrorMessage(keyval.Key);
                    }
                }
            }
        }

        protected bool ParameterIsValid(string param_name, string param_value)
        {
            //double
            if (Dictionaries.parameterDoubleLimits.ContainsKey(param_name))
            {
                double result;
                if(!double.TryParse(param_value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
                    return false;

                List<double> limits = Dictionaries.parameterDoubleLimits[param_name];
                if(result >= limits[0] && result <= limits[1])
                    return true;
                return false;
            }
            //int
            if (Dictionaries.parameterIntLimits.ContainsKey(param_name))
            {
                int result;
                if (!Int32.TryParse(param_value, out result))
                    return false;

                List<int> limits = Dictionaries.parameterIntLimits[param_name];
                if (result >= limits[0] && result <= limits[1])
                    return true;
                return false;
            }
            //other
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

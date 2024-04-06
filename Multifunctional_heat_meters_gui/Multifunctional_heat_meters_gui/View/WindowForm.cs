using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using System.Text.RegularExpressions;

namespace Multifunctional_heat_meters_gui.View
{
    public class WindowForm : Box
    {
        private Builder _builder;

        public event EventHandler<EventsArgs.NextFormArgs> NextFormEvent;
        public event EventHandler<EventsArgs.NextFormArgs> PreviousFormEvent;
        public event EventHandler SaveFormEvent;

        protected string _formName;
        protected int _formIndex = 0;
        protected bool AutoCheckFlag;

        protected Dictionary<string, Widget> parameter_widget;

        protected BackForwardComponent _backForwardComponent;
        protected Dictionary<string, string> paramsToNextForm = new Dictionary<string, string>();

        protected WindowForm(string formName, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _formName = formName;
            _backForwardComponent = BackForwardComponent.Create();

            SetupHandlers();
        }

        protected virtual void OnNextFormAction() { }
        protected virtual void OnPreviousFormAction() { }

        protected virtual bool IsAbleToGoToNext()
        {
            return true;
        }

        protected void SetupHandlers()
        {
            _backForwardComponent.BackButtonClickedEvent += new EventHandler(GoToPreviousForm);
            _backForwardComponent.ForwardButtonClickedEvent += new EventHandler(GoToNextForm);
            _backForwardComponent.SaveButtonClickedEvent += new EventHandler(SaveData);

            //_backForwardComponent.ValueCheckButtonActiveEvent += new EventHandler(SetValueCheckActive);
            //_backForwardComponent.ValueCheckButtonInactiveEvent += new EventHandler(SetValueCheckInactive);
        }

        public virtual bool IsFormFilledOut()
        {
            return true;
        }

        public virtual void SetAutoValueCheck(bool flag)
        {
            AutoCheckFlag = flag;
        }

        protected void OnValueChanged(object sender, List<string> e)
        {
            if (AutoCheckFlag == true)
            {
                string param_name = e[0];
                string param_value = e[1];
                if (!ParameterIsValid(param_name, param_value))
                    ShowErrorMessage(param_name);
                else
                    HideErrorMessage(param_name);
            }
            OnFormChanged(sender, EventArgs.Empty);
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

        protected bool ParameterIsValid(string param_name, string param_value)
        {
            //double
            if (Dictionaries.parameterDoubleLimits.ContainsKey(param_name))
            {
                double result;
                if (!double.TryParse(param_value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
                    return false;

                List<double> limits = Dictionaries.parameterDoubleLimits[param_name];
                if (result >= limits[0] && result <= limits[1])
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

        public virtual void OnFormChanged(object sender, EventArgs e)
        {
            if (IsFormFilledOut())
            {
                _backForwardComponent.SetForwardButtonSensitive();
            }
            else
            {
                _backForwardComponent.SetForwardButtonInsensitive();
            }
        }

        public virtual void OnLoadForm(EventsArgs.NextFormArgs paramsFromPreviousForm, AppState appState)
        {
            OnFormChanged(this, EventArgs.Empty);
        }

        protected bool CheckParameterValidation(string param_name)
        {
            return true;
        }

        public string FormName => _formName;
        public int FormIndex => _formIndex;

        private void GoToNextForm(object sender, EventArgs e)
        {
            OnNextFormAction();
            
            if (IsAbleToGoToNext())
            {
                EventsArgs.NextFormArgs args = new EventsArgs.NextFormArgs(paramsToNextForm);
                NextFormEvent?.Invoke(this, args);
            }
        }

        private void GoToPreviousForm(object sender, EventArgs e)
        {
            OnPreviousFormAction();

            //if (IsAbleToGoToPrevious())
            //{
                EventsArgs.NextFormArgs args = new EventsArgs.NextFormArgs(paramsToNextForm);
                PreviousFormEvent?.Invoke(this, args);
            //}
        }
        protected void TurnIntoNumber(object sender, EventArgs e)
        {
            Entry temp = (Entry)sender;

            string text = temp.Text;
            string numberOnly = Regex.Replace(text, "[^0-9. ,-]", "");
            numberOnly = numberOnly.Replace(",", ".");
            temp.Text = numberOnly;
        }

        private void SaveData(object sender, EventArgs e)
        {
            SaveFormEvent?.Invoke(this, e);
        }
    }
}

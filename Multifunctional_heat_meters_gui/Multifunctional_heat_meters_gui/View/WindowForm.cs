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
        }

        public virtual void OnLoadForm(EventsArgs.NextFormArgs paramsFromPreviousForm, AppState appState)
        {

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
            temp.Text = numberOnly;
        }

        private void SaveData(object sender, EventArgs e)
        {
            SaveFormEvent?.Invoke(this, e);
        }
    }
}

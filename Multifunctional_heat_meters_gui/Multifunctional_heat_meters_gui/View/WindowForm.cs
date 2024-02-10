using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class WindowForm : Box
    {
        private Builder _builder;

        public event EventHandler<EventsArgs.NextFormArgs> NextFormEvent;
        public event EventHandler<EventsArgs.NextFormArgs> PreviousFormEvent;

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
            _backForwardComponent.BackButtonClickedEvent += new EventHandler(GoToPreviousForm);
            _backForwardComponent.ForwardButtonClickedEvent += new EventHandler(GoToNextForm);

            SetupHandlers();
        }

        protected virtual void OnNextFormAction() { }
        protected virtual void OnPreviousFormAction() { }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }

        public virtual void OnLoadForm(EventsArgs.NextFormArgs paramsFromPreviousForm, AppState appState)
        {

        }

        public string FormName => _formName;
        public int FormIndex => _formIndex;

        private void GoToNextForm(object sender, EventArgs e)
        {
            Console.WriteLine("GoToNextForm");
            OnNextFormAction();
            
            //if (IsAbleToGoToNext())
            //{
                EventsArgs.NextFormArgs args = new EventsArgs.NextFormArgs(paramsToNextForm);
                NextFormEvent?.Invoke(this, args);
            //}
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
    }
}

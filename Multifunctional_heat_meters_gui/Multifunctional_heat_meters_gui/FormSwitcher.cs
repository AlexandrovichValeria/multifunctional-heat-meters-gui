using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui
{
    class FormSwitcher
    {
        private View.ContentMenu _menu;
        private LinkedList<View.WindowForm> _forms;
        private Box _contentBox;
        private LinkedListNode<View.WindowForm> _head;
        private AppState _appState;

        public FormSwitcher(View.ContentMenu menu, AppState appState, Box contentBox)
        {
            _menu = menu;
            _forms = appState.GetForms();
            _appState = appState;
            _contentBox = contentBox;

            _menu.FormChanged += new EventHandler(ChangeFormByClickOnMenu);

            foreach (View.WindowForm form in _forms)
            {
                form.NextFormEvent += new EventHandler<EventsArgs.NextFormArgs>(GoAhead);
                form.PreviousFormEvent += new EventHandler<EventsArgs.NextFormArgs>(GoBack);
            }
            _head = _forms.First;
            SetForm(_head.Value);
            _head.Value.OnLoadForm(null, _appState);
            _menu.SelectButtonByName(_head.Value.FormName);
        }

        public void SetEventListenersForForm(object form, EventArgs args)
        {
            View.WindowForm _form = (View.WindowForm)form;
            _form.NextFormEvent += new EventHandler<EventsArgs.NextFormArgs>(GoAhead);
            _form.PreviousFormEvent += new EventHandler<EventsArgs.NextFormArgs>(GoBack);
        }

        private LinkedListNode<View.WindowForm> GetFormNodeByName(string name)
        {
            LinkedListNode<View.WindowForm> currentNode = _forms.First;

            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;

                if (currentForm.FormName == name)
                {
                    return currentNode;
                }

                currentNode = currentNode.Next;
            }

            return null;
        }

        private void SetForm(View.WindowForm subForm)
        {
            foreach (var child in _contentBox.Children){
                _contentBox.Remove(child);
            }
            _contentBox.Add(subForm);
            subForm.Show();
        }

        public void ChangeFormByClickOnMenu(object sender, EventArgs e)
        {
            TreeSelection selection = (TreeSelection)sender;
            ITreeModel model;
            TreeIter iter;
            string textValue = "";
            if (selection.GetSelected(out model, out iter))
            {
                textValue = (string)model.GetValue(iter, 0);
            }
            if (textValue == Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SYSTEM])
                textValue = "Общесистемные параметры 1";
            else if (textValue == Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SENSORS])
                textValue = "Общесистемные параметры 2";

            LinkedListNode<View.WindowForm> formNode = GetFormNodeByName(textValue);
            _head = formNode;

            if (formNode != null)
            {
                View.WindowForm currentForm = formNode.Value;
                currentForm.OnLoadForm(null, _appState);
                SetForm(currentForm);
            }
            
        }

        public void GoBack(object sender, EventsArgs.NextFormArgs e)
        {
            LinkedListNode<View.WindowForm> previousFormNode = _head.Previous;

            if (previousFormNode != null)
            {
                View.WindowForm previousForm = previousFormNode.Value;

                SetForm(previousForm);
                previousForm.OnLoadForm(e, _appState);
                string name = previousForm.FormName;
                if (name == "Общесистемные параметры 1")
                    name = Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SYSTEM];
                if (name == "Общесистемные параметры 2")
                    name = Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SENSORS];
                _menu.SelectButtonByName(name);
            }
        }

        public void GoAhead(object sender, EventsArgs.NextFormArgs e)
        {
            LinkedListNode<View.WindowForm> nextFormNode = _head.Next;

            if (nextFormNode != null)
            {
                View.WindowForm nextForm = nextFormNode.Value;

                SetForm(nextForm);
                nextFormNode.Value.OnLoadForm(e, _appState);
                string name = nextForm.FormName;
                if (name == "Общесистемные параметры 1")
                    name = Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SYSTEM];
                if (name == "Общесистемные параметры 2")
                    name = Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SENSORS];
                _menu.SelectButtonByName(name);
            }
            else 
            {
                /*View.WindowForm nextForm = _forms.First.Value;

                SetForm(nextForm);
                nextForm.OnLoadForm(e, _appState);
                _menu.SelectButtonByName("Настройка датчиков общесистемного канала");*/
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class CheckboxesBlock : Box
    {
        private Builder _builder;
        [Builder.Object]
        private Grid Checkboxes;

        public event EventHandler CheckBoxesChecked;

        private static int s_countCheckboxesInLine = 8;

        private CheckButton[] _checkboxes;
        private Label[] _labels;
        private string _result = "";
        public string Result
        {
            get { return _result; }
        }

        public static CheckboxesBlock Create(int countCheckboxes, string prefix, string stateString)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.CheckboxesBlock.glade", null);
            return new CheckboxesBlock(countCheckboxes, prefix, stateString, builder, builder.GetObject("box").Handle);
        }
        protected CheckboxesBlock(int countCheckboxes, string prefix, string stateString, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _checkboxes = new CheckButton[countCheckboxes];
            _labels = new Label[countCheckboxes];

            for (int i = 0; i < countCheckboxes; i++)
            {
                CheckButton currentCheckbox = new CheckButton($"{prefix}{i + 1}");
                bool state;
                if (stateString.Length > 0)
                    state = stateString[i] == '1' ? true : false;
                else
                    state = false;
                currentCheckbox.Active = state;

                _result += state ? "1" : "0";

                currentCheckbox.Clicked += new EventHandler(Checkbox_Checked);

                _checkboxes[i] = currentCheckbox;
                int row = 1;
                if (i >= 8)
                    row = 2;
                Checkboxes.Attach(currentCheckbox, i%8+1, row, 1, 1);
            }

            ShowAll();

            SetupHandlers();
        }

        public void SetCheckboxes(string data)
        {
            _result = data;
            for(int i = 0; i < data.Length; i++)
            {
                if (data[i] == '1')
                    _checkboxes[i].Active = true;
                else
                    _checkboxes[i].Active = false;
            }
        }

        public void SelectAll()
        {
            foreach(CheckButton checkbox in _checkboxes)
            {
                checkbox.Active = true;
            }
        }

        public void DeselectAll()
        {
            foreach (CheckButton checkbox in _checkboxes)
            {
                checkbox.Active = false;
            }
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
        }
        private void Checkbox_Checked(object sender, EventArgs e)
        {
            _result = "";

            foreach (CheckButton checkbox in _checkboxes)
            {
                _result += checkbox.Active ? "1" : "0";
            }
            CheckBoxesChecked(this, EventArgs.Empty);
        }

    }
}

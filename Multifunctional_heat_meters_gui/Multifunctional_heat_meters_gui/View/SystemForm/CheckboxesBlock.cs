using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class CheckboxesBlock : Grid
    {
        private Builder _builder;
        public event EventHandler CheckBoxesChecked;

        private static int s_countCheckboxesInLine = 8;

        private CheckButton[] _checkboxes;
        private Label[] _labels;
        private string _result = "";
        public string Result
        {
            get { return _result; }
        }

        public static CheckboxesBlock Create(int countCheckboxes, string prefix)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.CheckboxesBlock.glade", null);
            return new CheckboxesBlock(countCheckboxes, prefix, builder, builder.GetObject("Checkboxes").Handle);
        }
        protected CheckboxesBlock(int countCheckboxes, string prefix, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _checkboxes = new CheckButton[countCheckboxes];
            _labels = new Label[countCheckboxes];

            for (int i = 0; i < countCheckboxes; i++)
            {
                _result += "0";

                /*if (i % 8 == 0)
                {
                    Checkboxes.RowDefinitions.Add(new RowDefinition());
                }
                StackPanel panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };*/

                Label currentLabel = new Label($"{prefix}{i + 1}");

                CheckButton currentCheckbox = new CheckButton();
                /*{
                    Width = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsEnabled = true
                };*/

                
                currentCheckbox.Clicked += new EventHandler(Checkbox_Checked);
                

                Box box = new Box(Orientation.Horizontal, 0);


                _checkboxes[i] = currentCheckbox;
                _labels[i] = currentLabel;

                box.Add(currentCheckbox);
                box.Add(currentLabel);
                //panel.Children.Add(currentCheckbox);
                //panel.Children.Add(currentLabel);

                //Grid.SetRow(panel, i / s_countCheckboxesInLine);
                //Grid.SetColumn(panel, i % s_countCheckboxesInLine);
                int row = 1;
                if (i >= 8)
                    row = 2;
                Attach(box, i%8+1, row, 1, 1);
                
            }

            ShowAll();

            SetupHandlers();
        }

        /// <summary> Sets up the handlers. </summary>
        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
        }
        private void Checkbox_Checked(object sender, EventArgs e)
        {

            _result = "";

            foreach (CheckButton checkbox in _checkboxes)
            {
                _result += (bool)checkbox.Active ? "1" : "0";
            }
            CheckBoxesChecked(this, EventArgs.Empty);

        }
    }
}

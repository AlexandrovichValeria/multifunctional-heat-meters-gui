using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class ConsumerForm : WindowForm
    {
        public Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        public Label label1;
        [Builder.Object]
        public SpinButton spin_button1;
        [Builder.Object]
        public ComboBox combo1;
        [Builder.Object]
        private Grid pipeline_grid;

        private Dictionary<int, ComboBoxText> combo_numbers;

        public static ConsumerForm Create(List<int> pipelinesNumbers, int consumerNumber)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.ConsumerForm.glade", null);
            return new ConsumerForm(pipelinesNumbers, consumerNumber, builder, builder.GetObject("form_box").Handle);
        }

        protected ConsumerForm(List<int> pipelinesNumbers, int consumerNumber, Builder builder, IntPtr handle) : base($"п{consumerNumber}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _formIndex = consumerNumber;

            button_box.Add(_backForwardComponent);
            combo_numbers = new Dictionary<int, ComboBoxText>();

            label1.Text = "Потребитель " + consumerNumber.ToString();

            for (int i = 0; i < pipelinesNumbers.Count; i++)
            {
                Label label = new Label("Трубопровод №" + pipelinesNumbers[i].ToString());
                ComboBoxText comboBox = new ComboBoxText();
                comboBox.Name = "pipeline_combo" + pipelinesNumbers[i].ToString();
                comboBox.AppendText("Не задействован в данной схеме");
                comboBox.AppendText("Задействован как подающий");
                comboBox.AppendText("Задействован как обратный");
                comboBox.AppendText("Задействован как подпитка или трубопровод ГВС");

                comboBox.Active = 0;
                combo_numbers.Add(pipelinesNumbers[i], comboBox);

                if (i < 8)
                {
                    pipeline_grid.Attach(label, 0, i, 1, 1);
                    pipeline_grid.Attach(comboBox, 1, i, 1, 1);
                }
                else {
                    pipeline_grid.Attach(label, 2, i-8, 1, 1);
                    pipeline_grid.Attach(comboBox, 3, i-8, 1, 1);
                }
            }

            SetupHandlers();
            ShowAll();
        }

        public Dictionary<string, string> GetConsumerSettings()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "consumer_id", $"{spin_button1.Value}" },
                { "accountingSchemeNumber", $"{combo1.Active}" },
            };
            foreach(int pipeline_number in combo_numbers.Keys)
            {
                res.Add($"{pipeline_number}", $"{combo_numbers[pipeline_number].Active}");
            }
            return res;
        }
        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
        }

        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }


    }
}

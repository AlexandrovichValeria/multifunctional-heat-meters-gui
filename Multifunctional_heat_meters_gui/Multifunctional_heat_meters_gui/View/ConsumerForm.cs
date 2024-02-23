using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
        public SpinButton spinbutton1;
        [Builder.Object]
        public ComboBox combo1;
        [Builder.Object]
        private Grid pipeline_grid;

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

            label1.Text = "Потребитель №" + consumerNumber.ToString();

            for (int i = 0; i < pipelinesNumbers.Count; i++)
            {
                Label label = new Label("Трубопровод №" + pipelinesNumbers[i].ToString());
                ComboBoxText comboBox = new ComboBoxText();
                comboBox.Name = "pipeline_combo" + i.ToString();
                comboBox.AppendText("Не задействован в данной схеме");
                comboBox.AppendText("Задействован как подающий");
                comboBox.AppendText("Задействован как обратный");
                comboBox.AppendText("Задействован как подпитка или трубопровод ГВС");

                pipeline_grid.Attach(label, 0, i, 1, 1);
                pipeline_grid.Attach(comboBox, 1, i, 1, 1);
            }

            SetupHandlers();
            ShowAll();
        }

        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            combo1.Changed += OnComboChanged;
        }

        protected void OnComboChanged(object sender, EventArgs a)
        {
            //Console.WriteLine("AAAAAAAAA");
            //Console.WriteLine(combo1.ActiveId);
        }

        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }


    }
}

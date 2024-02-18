using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class PipelineSettings1Form : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private Label label1;
        [Builder.Object]
        private Label label4;
        [Builder.Object]
        private Label label5;
        [Builder.Object]
        private Label label6;
        [Builder.Object]
        private Label label7;
        [Builder.Object]
        private Label label8;
        [Builder.Object]
        private Label label9;
        [Builder.Object]
        private Label label10;
        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;
        [Builder.Object]
        private Entry entry3;
        [Builder.Object]
        private Entry entry4;
        [Builder.Object]
        private Entry entry5;
        [Builder.Object]
        private Entry entry6;
        [Builder.Object]
        private Entry entry7;
        [Builder.Object]
        private Entry entry8;
        [Builder.Object]
        private Entry entry9;
        [Builder.Object]
        private Entry entry10;
        [Builder.Object]
        private Entry entry11;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private ComboBoxText combo2;


        public string curIndicator = "01";
        
        public static PipelineSettings1Form Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.PipelineSettings1Form.glade", null);
            return new PipelineSettings1Form(index, builder, builder.GetObject("form_box").Handle);
        }

        protected PipelineSettings1Form(int index, Builder builder, IntPtr handle) : base($"Первая настройка трубопровода {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            button_box.Add(_backForwardComponent);
            SetupHandlers();
        }

        public Dictionary<string, string> GetPipelineSettings1()
        {
            string[] cv1 = { "040", "041", "042" };

            string combo1Value = ""; //"042" по умолчанию
            if (combo1.ActiveId != "-1")
                combo1Value = cv1[Int32.Parse(combo1.ActiveId)];

            string[] cv2 = { "023", "024", "033", "034", "043", "044", "053", "054", "063", "064" };
            string combo2Value = ""; //"043" по умолчанию
            if (combo2.ActiveId != "-1")
                combo2Value = cv2[Int32.Parse(combo2.ActiveId)];

            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "109н00", $"{entry5.Text}" }, //константное значение расхода т/час или м^3/час
                { "032н00", $"{combo1Value}" }, //выходной сигнал датчика избыточного давления 
                { "032н01", $"{entry6.Text}" }, //верхний предел мПА или кгС/см2
                { "032н08", $"{entry7.Text}" }, //поправка на высоту столба в имп. трубке датч. давления мПА или кгС/см2
                { "113н00", $"{entry8.Text}" }, //константное значение абсолютного давления мПА или кгС/см2
                { "033н00", $"{combo2Value}" }, //признак подключения и тип датчика
                { "033н01", $"{entry10.Text}" }, //верхний предел ºС
                { "033н02", $"{entry9.Text}" }, //нижний предел ºС
                { "114н00", $"{entry11.Text}" }, //константное значение температуры теплоносителя ºС
            };
            if (entry4.IsVisible)
            {
                res.Add("034н01", $"{entry2.Text}"); //верхний предел по паспорту прибора м3/час или т/час (?)
                res.Add("034н02", $"{entry1.Text}"); //нижний предел по паспорту прибора м3/час или т/час (?)
                res.Add("034н06", $"{entry4.Text}"); //верхний предел частоты входного сигнала 
                res.Add("034н07", $"{entry3.Text}"); //нижний предел частоты входного сигнала
            }
            else
            {
                res.Add("034н06", $"{entry2.Text}"); //верхний предел по паспорту прибора 
                res.Add("034н07", $"{entry1.Text}"); //нижний предел по паспорту прибора
                res.Add("034н08", $"{entry3.Text}"); //цена импульса - из паспорта прибора м³ или т
            }
            return res;
        }

        public void SetWindow()
        {
            if (curIndicator == "01" || curIndicator == "02")
            {
                label1.Text = "Укажите нижний и верхний предел частоты входного сигнала.";
                entry2.Text = "700";

                label6.Text = "Введите цену импульса из паспорта прибора";
                label7.Hide();
                label8.Hide();
                entry4.Hide();

                label4.Text = "Гц";
                label5.Text = "Гц";

                label9.Text = "м³";
                label10.Hide();
            }

            else if (curIndicator == "03" || curIndicator == "04")
            {
                label1.Text = "Нижний и верхний диапазон измерений по паспорту прибора. Нижний предел диапазона измерений должен соответствовать настройкам выхода расходомера.";
                entry2.Text = "763.400";

                label6.Text = "Нижний и верхний предел частоты входного сигнала.";
                label7.Show();
                label8.Show();
                entry4.Show();

                label4.Text = "м³/час";
                label5.Text = "м³/час";

                label9.Text = "Гц";
                label10.Show();
            }
        }

        public override void OnLoadForm(EventsArgs.NextFormArgs paramsFromPreviousForm, AppState appState)
        {
            if (paramsFromPreviousForm == null)
            {
                return;
            }

            if (paramsFromPreviousForm.Params.ContainsKey("curIndicator"))
            {
                curIndicator = paramsFromPreviousForm.Params["curIndicator"];
            }
            else
            {
                curIndicator = "01";
            }

            SetWindow();
        }

        protected void SetupHandlers()
        {
            
        }

    }
}

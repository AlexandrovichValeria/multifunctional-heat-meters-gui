﻿using System;
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
        private Label label2;
        [Builder.Object]
        private Label entry_label1;
        [Builder.Object]
        private Label entry_label2;
        [Builder.Object]
        private Label entry_label3;
        [Builder.Object]
        private Label entry_label4;
        [Builder.Object]
        private Label measure_label1;
        [Builder.Object]
        private Label measure_label2;
        [Builder.Object]
        private Label measure_label3;
        [Builder.Object]
        private Label measure_label4;
        [Builder.Object]
        private Label measure_label5;
        [Builder.Object]
        private Label measure_label6;
        [Builder.Object]
        private Label measure_label7;
        [Builder.Object]
        private Label measure_label8;
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

        [Builder.Object]
        private Label name_label;


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

            _formIndex = index;
            button_box.Add(_backForwardComponent);
            name_label.Text = "Трубопровод " + index.ToString();
            SetupHandlers();
        }

        public Dictionary<string, string> GetPipelineSettings1()
        {
            //Console.WriteLine("GetPipelineSettings1");
            string[] cv1 = { "040", "041", "042" };

            string combo1Value = ""; //"042" по умолчанию
            if (combo1.ActiveId != "-1")
                combo1Value = cv1[Int32.Parse(combo1.ActiveId)];

            string[] cv2 = { "023", "024", "033", "034", "043", "044", "053", "054", "063", "064" };
            string combo2Value = ""; //"043" по умолчанию
            if (combo2.ActiveId != "-1")
                combo2Value = cv2[Int32.Parse(combo2.ActiveId)];

            string value109н01 = "034";
            if(_formIndex < 10)
                value109н01 += "0" + _formIndex.ToString();
            else
                value109н01 += _formIndex.ToString();

            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "109н00", $"{entry5.Text}" }, //константное значение расхода т/час или м^3/час
                { "109н01", $"{value109н01}" }, //??
                { "032н00", $"{combo1Value}" }, //выходной сигнал датчика избыточного давления 
                { "032н01", $"{entry6.Text}" }, //верхний предел мПА или кгС/см2
                { "032н08", $"{entry7.Text}" }, //поправка на высоту столба в имп. трубке датч. давления мПА или кгС/см2
                { "113н00", $"{entry8.Text}" }, //константное значение абсолютного давления мПА или кгС/см2
                { "033н00", $"{combo2Value}" }, //признак подключения и тип датчика
                { "033н01", $"{entry10.Text}" }, //верхний предел ºС
                { "033н02", $"{entry9.Text}" }, //нижний предел ºС
                { "114н00", $"{entry11.Text}" }, //константное значение температуры теплоносителя ºС
            };
            if (curIndicator == "03" || curIndicator == "04") //с частотным
            {
                res.Add("034н01", $"{entry2.Text}"); //верхний предел по паспорту прибора м3/час или т/час
                res.Add("034н02", $"{entry1.Text}"); //нижний предел м3/час или т/час (только для частотного выходного сигнала.) (для остальных всегда 0)
                res.Add("034н06", $"{entry4.Text}"); //верхний предел частоты входного сигнала 
                res.Add("034н07", $"{entry3.Text}"); //нижний предел частоты входного сигнала
            }
            else //с числоимпульсным
            {
                res.Add("034н06", $"{entry1.Text}"); //верхний предел по паспорту прибора 
                res.Add("034н07", $"{0}"); //нижний предел по паспорту прибора
                res.Add("034н08", $"{entry3.Text}"); //цена импульса - из паспорта прибора м³/имп или т/имп
            }
            return res;
        }

        public override bool IsFormFilledOut() //проверить
        {
            Dictionary<string, string> pars = GetPipelineSettings1();
            if (pars["109н00"] == "" || pars["032н00"] == "" || pars["032н01"] == "" || pars["032н08"] == "" ||
                pars["113н00"] == "" || pars["033н00"] == "" || pars["033н01"] == "" || pars["033н02"] == "" ||
                pars["114н00"] == "")
                return false;

            if (pars.ContainsKey("034н01")) //с частотным
            {
                if (pars["034н01"] == "" || pars["034н02"] == "" || pars["034н06"] == "" || pars["034н07"] == "")
                    return false;
                return true;
            }
            if (pars["034н06"] == "" || pars["034н07"] == "" || pars["034н08"] == "")
                return false;
            return true;
        }

        public void SetWindow()
        {
            if (curIndicator == "01") //объема с числоимпульсным
            {
                label1.Text = "Верхний предел";
                entry_label1.Hide();
                entry1.Text = "700";
                measure_label1.Text = "м³/час";
                entry_label2.Hide();
                entry2.Hide();
                measure_label2.Hide();

                label2.Text = "Цена импульса из паспорта прибора";
                entry_label3.Hide();
                entry_label4.Hide();
                entry4.Hide();

                measure_label3.Text = "м³/имп";
                measure_label4.Hide();

                measure_label5.Text = "м³/час";
            }

            else if(curIndicator == "02") //массы с числоимпульсным
            {
                label1.Text = "Верхний предел";

                entry_label1.Hide();
                entry1.Text = "700";
                measure_label1.Text = "т/час";
                entry_label2.Hide();
                entry2.Hide();
                measure_label2.Hide();

                label2.Text = "Цена импульса из паспорта прибора";
                entry_label3.Hide();
                entry_label4.Hide();
                entry4.Hide();

                measure_label3.Text = "т/имп";
                measure_label4.Hide();

                measure_label5.Text = "т/час";
            }

            else if (curIndicator == "03") //объема с частотным
            {
                entry_label1.Show();

                label1.Text = "Нижний и верхний диапазон измерений по паспорту прибора. Нижний предел диапазона измерений должен соответствовать настройкам выхода расходомера";
                entry1.Text = "0";
                measure_label1.Text = "м³/час";

                entry_label2.Show();
                entry2.Show();
                measure_label2.Show();
                entry2.Text = "763.400";
                measure_label2.Text = "м³/час";

                label2.Text = "Нижний и верхний предел частоты входного сигнала";

                entry_label3.Show();
                measure_label3.Text = "Гц";

                entry_label4.Show();
                entry4.Show();
                measure_label4.Show();
                measure_label4.Text = "Гц";
                measure_label5.Text = "м³/час";
            }
            else if (curIndicator == "04") //массы с частотным
            {
                entry_label1.Show();

                label1.Text = "Нижний и верхний диапазон измерений по паспорту прибора. Нижний предел диапазона измерений должен соответствовать настройкам выхода расходомера";
                entry1.Text = "0";
                measure_label1.Text = "т/час";

                entry_label2.Show();
                entry2.Show();
                measure_label2.Show();
                entry2.Text = "763.400";
                measure_label2.Text = "т/час";

                label2.Text = "Нижний и верхний предел частоты входного сигнала";

                entry_label3.Show();
                measure_label3.Text = "Гц";

                entry_label4.Show();
                entry4.Show();
                measure_label4.Show();
                measure_label4.Text = "Гц";
                measure_label5.Text = "т/час";
            }
        }

        protected override bool IsAbleToGoToNext()
        {
            if (!IsFormFilledOut())
                return false;
            string result = GetPipelineSettings1().ContainsKey("034н02") ? GetPipelineSettings1()["034н02"] : "0";
            if ("" == "")
            {
                paramsToNextForm = new Dictionary<string, string>()
                {
                    { "lowLimit", result }
                };
                return true;
            }

            return false;
        }

        

        public override void OnLoadForm(EventsArgs.NextFormArgs paramsFromPreviousForm, AppState appState)
        {
            OnFormChanged(this, EventArgs.Empty);
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

        public void ChangePressureMeasurement(int unitOfMeasurement)
        {
            switch (unitOfMeasurement)
            {
                case 0:
                    measure_label6.Text = "МПа";
                    measure_label7.Text = "МПа";
                    measure_label8.Text = "МПа";
                    break;
                case 1:
                    measure_label6.Text = "кгс/см2";
                    measure_label7.Text = "кгс/см2";
                    measure_label8.Text = "кгс/см2";
                    break;
            }
        }

        public void ChangeSensorType(int sensorType)
        {

        }


        public void ChangePowerMeasurement(int unitOfMeasurement)
        {

        }

        protected void SetupHandlers()
        {
            entry1.Changed += TurnIntoNumber;
            entry2.Changed += TurnIntoNumber;
            entry3.Changed += TurnIntoNumber;
            entry4.Changed += TurnIntoNumber;
            entry5.Changed += TurnIntoNumber;
            entry6.Changed += TurnIntoNumber;
            entry7.Changed += TurnIntoNumber;
            entry8.Changed += TurnIntoNumber;
            entry9.Changed += TurnIntoNumber;
            entry10.Changed += TurnIntoNumber;
            entry11.Changed += TurnIntoNumber;

            entry1.Changed += OnFormChanged;
            //entry2.Changed += OnFormChanged;
            entry3.Changed += OnFormChanged;
            entry4.Changed += OnFormChanged;
            entry5.Changed += OnFormChanged;
            entry6.Changed += OnFormChanged;
            entry7.Changed += OnFormChanged;
            entry8.Changed += OnFormChanged;
            entry9.Changed += OnFormChanged;
            entry10.Changed += OnFormChanged;
            entry11.Changed += OnFormChanged;
        }

    }
}

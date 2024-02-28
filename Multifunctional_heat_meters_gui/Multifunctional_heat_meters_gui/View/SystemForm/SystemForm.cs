using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class SystemForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box Checkboxes_box;
        [Builder.Object]
        private Box button_box;

        [Builder.Object]
        private ComboBoxText pressure_combo;
        [Builder.Object]
        private ComboBoxText power_combo;

        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;
        [Builder.Object]
        private Entry entry3;
        [Builder.Object]
        private Entry entry4;
        [Builder.Object]
        private CheckButton check1;
        [Builder.Object]
        private CheckButton check2;
        [Builder.Object]
        private CheckButton check3;
        [Builder.Object]
        private CheckButton check4;
        [Builder.Object]
        private CheckButton check5;

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
        private CheckButton spec1_checkbox;
        [Builder.Object]
        private CheckButton spec2_checkbox;
        [Builder.Object]
        private Entry spec1;
        [Builder.Object]
        private Entry spec2;

        [Builder.Object]
        private Label energy_discr_system;


        private ParticipatedPipelinesBlock _participatedPipelinesBlock;
        private ADS_97_Form _ADS_97_Form;

        private int _minPipelinesCountFor_ADS_97 = 0;
        private static string SelectedPipelinesParam = "031н00";
        private Dictionary<string, string> ADS_97_result;

        public event EventHandler<EventsArgs.MeasurementEventArgs> PressureSystemChangedEvent;
        public event EventHandler<EventsArgs.MeasurementEventArgs> PowerSystemChangedEvent;

        public static SystemForm Create(Model.Device device)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SystemForm.glade", null);
            return new SystemForm(device, builder, builder.GetObject("form_box").Handle);
        }

        protected SystemForm(Model.Device device, Builder builder, IntPtr handle) : base("Общесистемные параметры", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _ADS_97_Form = ADS_97_Form.Create();

            if (device == Model.Device.SPT963)
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(16, 8);
            else
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(12, 6);
            Checkboxes_box.Add(_participatedPipelinesBlock);

            CalculateMinPipelinesCountForm_ADS_97(device);
            button_box.Add(_backForwardComponent);
            spec1.IsEditable = false;
            spec1.CanFocus = false;
            spec2.IsEditable = false;
            spec2.CanFocus = false;

            SetupHandlers();
        }

        public string GetParamFromWindow(string param)
        {
            Dictionary<string, string> result = GetSystemWindowData();
            return result.ContainsKey(param) ? result[param] : null;
        }
        public Dictionary<string, string> GetSystemWindowData()
        {
            Dictionary<string, string> result = _participatedPipelinesBlock.GetResult();

            result.Add("030н00", $"{pressure_combo.ActiveId}{power_combo.ActiveId}");
            
            result.Add("030н01", entry5.Text);
            result.Add("030н02", entry6.Text);
            result.Add("024", entry7.Text);
            result.Add("025", entry8.Text);
            result.Add("008", entry9.Text);

            result.Add("003", spec1.Text);
            result.Add("004", spec2.Text);

            result.Add("035н00", entry1.Text);
            result.Add("035н01", check1.Active ? "1" : "0");
            result.Add("036н00", entry2.Text);
            result.Add("036н01", check2.Active ? "1" : "0");
            result.Add("037н00", entry3.Text);
            result.Add("037н01", check3.Active ? "1" : "0");
            result.Add("040н00", entry4.Text);
            result.Add("040н01", check4.Active ? "1" : "0");

            result.Add("CurrentTimeAndDate", check5.Active ? "1" : "0");
            return result;
        }

        protected override void OnNextFormAction()
        {
            /*Console.WriteLine("SystemData");
            Dictionary<string, string> dic = GetSystemWindowData();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                Console.WriteLine($"{kvp.Key}, {kvp.Value}");
            }*/

            string zeroOneStringPipelines = GetParamFromWindow(SelectedPipelinesParam);
            int countSelectedPipelines = (zeroOneStringPipelines != null) ? GetPipelinesCountByOneZeroString(zeroOneStringPipelines) : 0;
            if (countSelectedPipelines > _minPipelinesCountFor_ADS_97)
            {
                _ADS_97_Form.Show();
                //MessageDialog dialog = new MessageDialog(_ADS_97_Form, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Dialog text");
            }
        }

        /*protected override bool IsAbleToGoToNext()
        {
            return !_ADS_97_Form.Visible;
        }*/

        private int GetPipelinesCountByOneZeroString(string oneZeroString)
        {
            int count = 0;
            foreach (char sym in oneZeroString)
            {
                if (sym == '1')
                    count++;
            }
            return count;
        }

        private void CalculateMinPipelinesCountForm_ADS_97(Model.Device device)
        {
            switch (device)
            {
                case Model.Device.SPT963:
                    _minPipelinesCountFor_ADS_97 = 8;
                    break;
                default:
                    _minPipelinesCountFor_ADS_97 = 4;
                    break;
            }
        }

        protected void SetupHandlers()
        {
            spec1_checkbox.Clicked += OnSpec1CheckBoxClicked;
            spec2_checkbox.Clicked += OnSpec2CheckBoxClicked;
            power_combo.Changed += OnPowerComboChanged;
            pressure_combo.Changed += OnPressureComboChanged;
            //DeleteEvent += OnLocalDeleteEvent;
        }

        protected void OnPowerComboChanged(object sender, EventArgs a)
        { 
            if(power_combo.ActiveId == "0")
            {
                energy_discr_system.Text = "ГДж";
            } 
            else if(power_combo.ActiveId == "1")
            {
                energy_discr_system.Text = "Гкал";
            }
            else if(power_combo.ActiveId == "2")
            {
                energy_discr_system.Text = "МВт*ч";
            }

            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(Int32.Parse(power_combo.ActiveId));

            PowerSystemChangedEvent?.Invoke(this, args);
        }

        protected void OnPressureComboChanged(object sender, EventArgs a)
        {
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(Int32.Parse(pressure_combo.ActiveId));

            PressureSystemChangedEvent?.Invoke(this, args);
        }

        protected void OnSpec1CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec1_checkbox.Active)
            {
                spec1.IsEditable = true;
                spec1.CanFocus = true;
            }
            else
            {
                spec1.IsEditable = false;
                spec1.CanFocus = false;
            }
        }
        protected void OnSpec2CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec2_checkbox.Active)
            {
                spec2.IsEditable = true;
                spec2.CanFocus = true;
            }
            else
            {
                spec2.IsEditable = false;
                spec2.CanFocus = false;
            }
        }
    }
}

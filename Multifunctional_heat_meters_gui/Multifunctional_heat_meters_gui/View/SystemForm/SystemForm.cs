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
        private Entry const_entry1;
        [Builder.Object]
        private Entry const_entry2;
        [Builder.Object]
        private Entry const_entry3;
        [Builder.Object]
        private Entry const_entry4;
        [Builder.Object]
        private CheckButton sensor_check1;
        [Builder.Object]
        private CheckButton sensor_check2;
        [Builder.Object]
        private CheckButton sensor_check3;
        [Builder.Object]
        private CheckButton sensor_check4;
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

        private int PressureMeasure;
        private int PowerMeasure;

        public static SystemForm Create(Model.Device device)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SystemForm.glade", null);
            return new SystemForm(device, builder, builder.GetObject("form_box").Handle);
        }

        protected SystemForm(Model.Device device, Builder builder, IntPtr handle) : base("Общесистемные параметры", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            PressureMeasure = 0;
            PowerMeasure = 0;
            _ADS_97_Form = ADS_97_Form.Create();

            if (device == Model.Device.SPT963)
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(16, 8);
            else
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(12, 6);
            Checkboxes_box.Add(_participatedPipelinesBlock);

            CalculateMinPipelinesCountForm_ADS_97(device);
            button_box.Add(_backForwardComponent);
            spec1.Sensitive = false;
            spec2.Sensitive = false;
            //DisableSensorsSettings();

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

            result.Add("035н00", const_entry1.Text);
            result.Add("035н01", sensor_check1.Active ? "1" : "0");
            result.Add("036н00", const_entry2.Text);
            result.Add("036н01", sensor_check2.Active ? "1" : "0");
            result.Add("037н00", const_entry3.Text);
            result.Add("037н01", sensor_check3.Active ? "1" : "0");
            result.Add("040н00", const_entry4.Text);
            result.Add("040н01", sensor_check4.Active ? "1" : "0");

            result.Add("CurrentTimeAndDate", check5.Active ? "1" : "0");
            return result;
        }

        public override bool IsFormFilledOut()
        {
            //if (!_participatedPipelinesBlock.SomeCheckboxesAreChecked())
              //  return false;
            Dictionary<string, string> pars = GetSystemWindowData();
            if (pars["030н00"] == "" || pars["030н01"] == "" || pars["030н02"] == "" || pars["030н02"] == ""
                || pars["024"] == "" || pars["025"] == "" /*|| pars["008"] == ""*/ || pars["003"] == "" || pars["004"] == "" 
                || pars["035н00"] == "" || pars["036н00"] == "" || pars["037н00"] == "" || pars["040н00"] == ""
                || pars["031н00"] == "" || pars["031н01"] == "")
                return false;
            return true;
        }

        protected override void OnNextFormAction()
        {
            string zeroOneStringPipelines = GetParamFromWindow(SelectedPipelinesParam);
            int countSelectedPipelines = (zeroOneStringPipelines != null) ? GetPipelinesCountByOneZeroString(zeroOneStringPipelines) : 0;
            if (countSelectedPipelines > _minPipelinesCountFor_ADS_97)
            {
                _ADS_97_Form.Show();
            }
            /*EventsArgs.MeasurementEventArgs powerArgs = new EventsArgs.MeasurementEventArgs(Int32.Parse(power_combo.ActiveId));
            PowerSystemChangedEvent?.Invoke(this, powerArgs);

            EventsArgs.MeasurementEventArgs pressureArgs = new EventsArgs.MeasurementEventArgs(Int32.Parse(power_combo.ActiveId));
            PressureSystemChangedEvent?.Invoke(this, pressureArgs);*/
        }

        protected override bool IsAbleToGoToNext()
        {
            return IsFormFilledOut();
        }

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

        public Dictionary<string, int> GetMeasurementSystems()
        {
            Dictionary<string, int> result = new Dictionary<string, int>(){
                { "Power", PowerMeasure },
                { "Pressure", PressureMeasure  }
            };
            return result;
        }

        
        public override void OnLoadForm(EventsArgs.NextFormArgs e, AppState appState)
        {
            OnFormChanged(this, EventArgs.Empty);
            if (appState.AreAllPipelinesFilledOut())
            {
                EnableSensorsSettings();
            } else
            {
                DisableSensorsSettings();
            }
        }


        public void EnableSensorsSettings()
        {
            const_entry1.Sensitive = true;
            const_entry2.Sensitive = true;
            const_entry3.Sensitive = true;
            const_entry4.Sensitive = true;
            sensor_check1.Sensitive = true;
            sensor_check2.Sensitive = true;
            sensor_check3.Sensitive = true;
            sensor_check4.Sensitive = true;
        }

        public void DisableSensorsSettings()
        {
            const_entry1.Sensitive = false;
            const_entry2.Sensitive = false;
            const_entry3.Sensitive = false;
            const_entry4.Sensitive = false;
            sensor_check1.Sensitive = false;
            sensor_check2.Sensitive = false;
            sensor_check3.Sensitive = false;
            sensor_check4.Sensitive = false;
        }

        protected void SetupHandlers()
        {
            spec1_checkbox.Clicked += OnSpec1CheckBoxClicked;
            spec2_checkbox.Clicked += OnSpec2CheckBoxClicked;
            power_combo.Changed += OnPowerComboChanged;
            pressure_combo.Changed += OnPressureComboChanged;

            const_entry1.Changed += TurnIntoNumber;
            const_entry2.Changed += TurnIntoNumber;
            const_entry3.Changed += TurnIntoNumber;
            const_entry4.Changed += TurnIntoNumber;
            entry5.Changed += TurnIntoNumber;
            entry6.Changed += TurnIntoNumber;
            entry7.Changed += TurnIntoNumber;
            entry8.Changed += TurnIntoNumber;
            entry9.Changed += TurnIntoNumber;
            spec1.Changed += TurnIntoNumber;
            spec2.Changed += TurnIntoNumber;

            _participatedPipelinesBlock.BlockChangedEvent += OnFormChanged;
            //power_combo.Changed += OnFormChanged;
            //pressure_combo.Changed += OnFormChanged;
            const_entry1.Changed += OnFormChanged;
            const_entry2.Changed += OnFormChanged;
            const_entry3.Changed += OnFormChanged;
            const_entry4.Changed += OnFormChanged;
            entry5.Changed += OnFormChanged;
            entry6.Changed += OnFormChanged;
            entry7.Changed += OnFormChanged;
            entry8.Changed += OnFormChanged;
            //entry9.Changed += OnFormChanged;
            spec1.Changed += OnFormChanged;
            spec2.Changed += OnFormChanged;

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

            PowerMeasure = Int32.Parse(power_combo.ActiveId);
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PowerMeasure);

            PowerSystemChangedEvent?.Invoke(this, args);
        }

        protected void OnPressureComboChanged(object sender, EventArgs a)
        {
            PressureMeasure = Int32.Parse(pressure_combo.ActiveId);
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PressureMeasure);

            PressureSystemChangedEvent?.Invoke(this, args);
        }

        protected void OnSpec1CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec1_checkbox.Active)
            {
                spec1.Sensitive = true;
                spec1.CanFocus = true;
            }
            else
            {
                spec1.Sensitive = false;
                spec1.CanFocus = false;
            }
        }
        protected void OnSpec2CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec2_checkbox.Active)
            {
                spec2.Sensitive = true;
                spec2.CanFocus = true;
            }
            else
            {
                spec2.Sensitive = false;
                spec2.CanFocus = false;
            }
        }
        
    }
}

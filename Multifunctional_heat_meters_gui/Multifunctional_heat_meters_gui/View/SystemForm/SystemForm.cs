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
        private Box sensor_box;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private Box other_settings_box;

        private ParticipatedPipelinesBlock _participatedPipelinesBlock;
        private ADS_97_Form _ADS_97_Form;
        private SensorBlock _sensorBlock;
        private OtherSettingsBlock _otherSettingsBlock;

        private int _minPipelinesCountFor_ADS_97 = 0;
        private static string SelectedPipelinesParam = "031н00";
        private Dictionary<string, string> ADS_97_result;

        public event EventHandler<EventsArgs.MeasurementEventArgs> PressureSystemChangedEvent;
        public event EventHandler<EventsArgs.MeasurementEventArgs> PowerSystemChangedEvent;

        private int PressureMeasure;
        private int PowerMeasure;

        public static SystemForm Create(int index, Model.Device device)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SystemForm.glade", null);
            return new SystemForm(index, device, builder, builder.GetObject("form_box").Handle);
        }

        protected SystemForm(int index, Model.Device device, Builder builder, IntPtr handle) : base($"Общесистемные параметры {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            PressureMeasure = 0;
            PowerMeasure = 0;

            _formIndex = index;

            _ADS_97_Form = ADS_97_Form.Create();
            _sensorBlock = SensorBlock.Create();
            _otherSettingsBlock = OtherSettingsBlock.Create();
            sensor_box.Add(_sensorBlock);
            other_settings_box.Add(_otherSettingsBlock);

            if (device == Model.Device.SPT963)
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(16, 8);
            else
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(12, 6);
            Checkboxes_box.Add(_participatedPipelinesBlock);

            CalculateMinPipelinesCountForm_ADS_97(device);
            button_box.Add(_backForwardComponent);

            if(index == 1)
            {
                _sensorBlock.DisableBlock();
                _participatedPipelinesBlock.EnableBlock();
                _otherSettingsBlock.EnableBlock();
            }
            else if (index == 2)
            {
                _sensorBlock.EnableBlock();
                _participatedPipelinesBlock.DisableBlock();
                _otherSettingsBlock.DisableBlock();
            }

            ShowAll();

            SetupHandlers();
        }

        public string GetParamFromWindow(string param)
        {
            Dictionary<string, string> result = GetSystemWindowData();
            return result.ContainsKey(param) ? result[param] : null;
        }

        public Dictionary<string, string> GetSystemWindowData()
        {
            Dictionary<string, string> pipelinesResult = _participatedPipelinesBlock.GetResult();
            Dictionary<string, string> sensorResult = _sensorBlock.GetResult();
            Dictionary<string, string> otherResult = _otherSettingsBlock.GetResult();

            Dictionary<string, string> result = pipelinesResult
                .Union(sensorResult)
                .Union(otherResult)
                .ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        public void SetSystemWindowData(Dictionary<string, string> data)
        {
            Dictionary<string, string> pipelinesResult = data.Where(s => s.Key == "031н00" || s.Key == "031н01")
                        .ToDictionary(dict => dict.Key, dict => dict.Value);
           
            _participatedPipelinesBlock.SetData(pipelinesResult);

            Dictionary<string, string> otherResult = data.Where(s => s.Key == "030н00" 
            || s.Key == "030н01" || s.Key == "030н02" || s.Key == "024" || s.Key == "025" || s.Key == "008" 
            || s.Key == "003" || s.Key == "004" || s.Key == "CurrentTimeAndDate")
                        .ToDictionary(dict => dict.Key, dict => dict.Value);

            _otherSettingsBlock.SetData(otherResult);

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
            if (_formIndex == 2)
            {
                View.SystemForm sysform1 = (View.SystemForm)appState.GetForms().First.Value;
                if (sysform1 != null)
                {
                    Dictionary<string, string> data = sysform1.GetSystemWindowData();
                    SetSystemWindowData(data);
                }
            }
            /*if (appState.AreAllPipelinesFilledOut())
            {
                //_sensorBlock.EnableBlock();
                //EnableSensorsSettings();
            } else
            {
                //_sensorBlock.DisableBlock();
                //DisableSensorsSettings();
            }*/
        }

        protected void SetupHandlers()
        {
            _participatedPipelinesBlock.BlockChangedEvent += OnFormChanged;
            _otherSettingsBlock.BlockChangedEvent += OnFormChanged;
            _otherSettingsBlock.PowerComboChangedEvent += OnPowerComboChanged;
            _otherSettingsBlock.PressureComboChangedEvent += OnPressureComboChanged;

            //DeleteEvent += OnLocalDeleteEvent;
        }

        protected void OnPowerComboChanged(object sender, EventArgs a)
        {
            PowerMeasure = _otherSettingsBlock.PowerMeasure;
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PowerMeasure);
            
            PowerSystemChangedEvent?.Invoke(this, args);
        }

        protected void OnPressureComboChanged(object sender, EventArgs a)
        {
            PressureMeasure = _otherSettingsBlock.PressureMeasure;
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PressureMeasure);

            PressureSystemChangedEvent?.Invoke(this, args);
        }

        
    }
}

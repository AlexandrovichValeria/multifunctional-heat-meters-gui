﻿using System;
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
        private SensorBlock _sensorBlock;
        private OtherSettingsBlock _otherSettingsBlock;

        private static string SelectedPipelinesParam = "031н00";
        private Dictionary<string, string> ADS_97_result;

        public event EventHandler<EventsArgs.MeasurementEventArgs> PressureSystemChangedEvent;
        public event EventHandler<EventsArgs.MeasurementEventArgs> PowerSystemChangedEvent;
        public event EventHandler<List<int>> OccupiedChannelsChangedEvent;
        public event EventHandler<Dictionary<string, string>> SystemFormChangedEvent;

        private int PressureMeasure;
        private int PowerMeasure;

        public static SystemForm Create(int index, Model.Device device, string state = "")
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SystemForm.glade", null);
            return new SystemForm(index, device, state, builder, builder.GetObject("form_box").Handle);
        }

        protected SystemForm(int index, Model.Device device, string state, Builder builder, IntPtr handle) : base($"Общесистемные параметры {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            PressureMeasure = 0;
            PowerMeasure = 0;

            _formIndex = index;
            string[] elements = state.Split(' ');
            string measureSystem = "";
            if (elements.Length >= 3)
            {
                measureSystem = elements[2];
            }

            _otherSettingsBlock = OtherSettingsBlock.Create(measureSystem);
            _sensorBlock = SensorBlock.Create(measureSystem);
            sensor_box.Add(_sensorBlock);
            other_settings_box.Add(_otherSettingsBlock);

            if (device == Model.Device.SPT963)
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(16, 8, state);
            else
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(12, 6, state);
            Checkboxes_box.Add(_participatedPipelinesBlock);

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

            SetupHandlers();

            OnPowerComboChanged(this, EventArgs.Empty);
            OnPressureComboChanged(this, EventArgs.Empty);
        }

        public string GetParamFromWindow(string param)
        {
            Dictionary<string, string> result = GetSystemWindowData();
            return result.ContainsKey(param) ? result[param] : null;
        }

        public List<int> GetSensorsState()
        {
            return _sensorBlock.GetSensorsState();
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
            Unsubscribe();
            if (_formIndex == 2)
            {
                Dictionary<string, string> pipelinesResult = data.Where(s => s.Key == "031н00" || s.Key == "031н01")
                            .ToDictionary(dict => dict.Key, dict => dict.Value);

                _participatedPipelinesBlock.SetData(pipelinesResult);

                Dictionary<string, string> otherResult = data.Where(s => s.Key == "030н00"
                || s.Key == "030н01" || s.Key == "030н02" || s.Key == "024" || s.Key == "025" || s.Key == "008"
                || s.Key == "003" || s.Key == "004" || s.Key == "CurrentTimeAndDate")
                            .ToDictionary(dict => dict.Key, dict => dict.Value);

                _otherSettingsBlock.SetData(otherResult);

                _sensorBlock.ChangePressureMeasurement(Int32.Parse(data["030н00"][0].ToString()));
            }

            else if (_formIndex == 1)
            {
                Dictionary<string, string> sensorResult = data.Where(s => s.Key == "035н00" || s.Key == "036н00"
                || s.Key == "037н00" || s.Key == "040н00" || s.Key == "sensor1" || s.Key == "sensor2"
                || s.Key == "sensor3" || s.Key == "sensor4").ToDictionary(dict => dict.Key, dict => dict.Value);

                _sensorBlock.SetData(sensorResult);
            }
            SetupHandlers();
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
            
            int selectedSensors032 = 0;
            int selectedSensors033 = 0;
            List<int> sensorState = GetSensorsState();
            if(sensorState[0] == 1)
                selectedSensors033++;
            if (sensorState[1] == 1)
                selectedSensors032++;
            if(sensorState[2] == 1)
                selectedSensors032++;
            if (sensorState[3] == 1)
                selectedSensors033++;
            int channels032 = countSelectedPipelines + selectedSensors032;
            int channels033 = countSelectedPipelines + selectedSensors033;
            OccupiedChannelsChangedEvent?.Invoke(this, new List<int> { channels032, channels033 });
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

        public override void SetAutoValueCheck(bool flag)
        {
            _sensorBlock.SetAutoValueCheck(flag);
            _otherSettingsBlock.SetAutoValueCheck(flag);
        }

        protected void SetupHandlers()
        {
            _otherSettingsBlock.PowerComboChangedEvent += OnPowerComboChanged;
            _otherSettingsBlock.PressureComboChangedEvent += OnPressureComboChanged;

            _participatedPipelinesBlock.BlockChangedEvent += OnFormChanged;
            _otherSettingsBlock.BlockChangedEvent += OnFormChanged;
            _sensorBlock.BlockChangedEvent += OnFormChanged;
            
            //DeleteEvent += OnLocalDeleteEvent;
        }

        protected void Unsubscribe()
        {
            _otherSettingsBlock.PowerComboChangedEvent -= OnPowerComboChanged;
            _otherSettingsBlock.PressureComboChangedEvent -= OnPressureComboChanged;

            _participatedPipelinesBlock.BlockChangedEvent -= OnFormChanged;
            _otherSettingsBlock.BlockChangedEvent -= OnFormChanged;
            _sensorBlock.BlockChangedEvent -= OnFormChanged;
        }

        protected void OnPowerComboChanged(object sender, EventArgs a)
        {
            PowerMeasure = _otherSettingsBlock.PowerMeasure;
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PowerMeasure);
            
            PowerSystemChangedEvent?.Invoke(this, args);

            OnFormChanged(this, EventArgs.Empty);
        }

        protected void OnPressureComboChanged(object sender, EventArgs a)
        {
            PressureMeasure = _otherSettingsBlock.PressureMeasure;
            _sensorBlock.ChangePressureMeasurement(PressureMeasure);
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(PressureMeasure);

            PressureSystemChangedEvent?.Invoke(this, args);

            OnFormChanged(this, EventArgs.Empty);
        }

        public override void OnFormChanged(object sender, EventArgs e)
        {
            if (IsFormFilledOut())
            {
                _backForwardComponent.SetForwardButtonSensitive();
            }
            else
            {
                _backForwardComponent.SetForwardButtonInsensitive();
            }
            Dictionary<string, string> data = new Dictionary<string, string>();

            data = GetSystemWindowData();

            if (_formIndex == 2)
            {
                List<int> sensors = GetSensorsState();
                for (int i = 0; i < sensors.Count; i++)
                {
                    data.Add($"sensor{i + 1}", sensors[i].ToString());
                }
            }

            SystemFormChangedEvent?.Invoke(this, data);
        }

        public void UpdateFromOtherForm(object sender, Dictionary<string, string> data)
        {
            SetSystemWindowData(data);
        }
    }
}

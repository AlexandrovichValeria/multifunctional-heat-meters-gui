﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class OtherSettingsBlock : WindowBlock
    {
        private Builder _builder;
        [Builder.Object]
        private ComboBoxText pressure_combo;
        [Builder.Object]
        private ComboBoxText power_combo;

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

        private int _pressureMeasure;
        private int _powerMeasure;

        public int PressureMeasure
        {
            get { return _pressureMeasure; }
        }
        public int PowerMeasure
        {
            get { return _powerMeasure; }
        }

        public event EventHandler<EventsArgs.MeasurementEventArgs> PressureComboChangedEvent;
        public event EventHandler<EventsArgs.MeasurementEventArgs> PowerComboChangedEvent;


        public static OtherSettingsBlock Create(string measureSystem = "00")
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.OtherSettingsBlock.glade", null);
            return new OtherSettingsBlock(measureSystem, builder, builder.GetObject("box").Handle);
        }
        protected OtherSettingsBlock(string measureSystem, Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            parameter_widget = new Dictionary<string, Entry>
            {
                { "030н01", entry5 },
                { "030н02", entry6 },
                { "024", entry7 },
                { "025", entry8 },
                { "008", entry9 },
                { "003", spec1 },
                { "004", spec2 },
            };

            spec1.Sensitive = false;
            spec2.Sensitive = false;

            if (measureSystem.Length == 2)
            {
                pressure_combo.ActiveId = measureSystem[0].ToString();
                power_combo.ActiveId = measureSystem[1].ToString();
                OnPressureComboChanged(this, EventArgs.Empty);
                OnPowerComboChanged(this, EventArgs.Empty);
            }

            ShowAll();
            SetupHandlers();
        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "030н00", $"{pressure_combo.ActiveId}{power_combo.ActiveId}" },
                { "030н01", ((Entry)parameter_widget["030н01"]).Text },
                { "030н02", ((Entry)parameter_widget["030н02"]).Text },
                { "024", ((Entry)parameter_widget["024"]).Text },
                { "025", ((Entry)parameter_widget["025"]).Text },
                { "008", ((Entry)parameter_widget["008"]).Text },
                { "003", ((Entry)parameter_widget["003"]).Text },
                { "004", ((Entry)parameter_widget["004"]).Text },
                { "CurrentTimeAndDate", check5.Active ? "1" : "0" },
            };
            return res;
        }

        public override void SetData(Dictionary<string, string> data)
        {
            pressure_combo.ActiveId = data["030н00"][0].ToString();
            power_combo.ActiveId = data["030н00"][1].ToString();
            parameter_widget["030н01"].Text = data["030н01"];
            parameter_widget["030н02"].Text = data["030н02"];
            parameter_widget["024"].Text = data["024"];
            parameter_widget["025"].Text = data["025"];
            parameter_widget["008"].Text = data["008"];
            parameter_widget["003"].Text = data["003"];
            parameter_widget["004"].Text = data["004"];

            if (data["CurrentTimeAndDate"] == "1")
                check5.Active = true;
            else
                check5.Active = false;
        }


        protected void SetupHandlers()
        {
            spec1_checkbox.Clicked += OnSpec1CheckBoxClicked;
            spec2_checkbox.Clicked += OnSpec2CheckBoxClicked;

            power_combo.Changed += OnPowerComboChanged;
            pressure_combo.Changed += OnPressureComboChanged;

            entry5.Changed += TurnIntoNumber;
            entry6.Changed += TurnIntoNumber;
            entry7.Changed += TurnIntoNumber;
            entry8.Changed += TurnIntoNumber;
            entry9.Changed += TurnIntoNumber;
            spec1.Changed += TurnIntoNumber;
            spec2.Changed += TurnIntoNumber;
            foreach (KeyValuePair<string, Entry> keyval in parameter_widget)
                parameter_widget[keyval.Key].Changed += (sender, e) => OnValueChanged(sender, new List<string> { keyval.Key, keyval.Value.Text });

        }
        
        protected void OnSpec1CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec1_checkbox.Active)
                spec1.Sensitive = true;
            else
                spec1.Sensitive = false;
        }
        protected void OnSpec2CheckBoxClicked(object sender, EventArgs a)
        {
            if (spec2_checkbox.Active)
                spec2.Sensitive = true;
            else
                spec2.Sensitive = false;
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
            
            _powerMeasure = Int32.Parse(power_combo.ActiveId);
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(_powerMeasure);

            PowerComboChangedEvent?.Invoke(this, args);
        }

        protected void OnPressureComboChanged(object sender, EventArgs a)
        {
            _pressureMeasure = Int32.Parse(pressure_combo.ActiveId);
            EventsArgs.MeasurementEventArgs args = new EventsArgs.MeasurementEventArgs(_pressureMeasure);

            PressureComboChangedEvent?.Invoke(this, args);
        }
        
    }
}

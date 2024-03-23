using System;
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
        

        public static OtherSettingsBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.OtherSettingsBlock.glade", null);
            return new OtherSettingsBlock(builder, builder.GetObject("box").Handle);
        }
        protected OtherSettingsBlock(Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            spec1.Sensitive = false;
            spec2.Sensitive = false;

            ShowAll();
            SetupHandlers();
        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "030н00", $"{pressure_combo.ActiveId}{power_combo.ActiveId}" },
                { "030н01", entry5.Text },
                { "030н02", entry6.Text },
                { "024", entry7.Text },
                { "025", entry8.Text },
                { "008", entry9.Text },
                { "003", spec1.Text },
                { "004", spec2.Text },
                { "CurrentTimeAndDate", check5.Active ? "1" : "0" },
            };
            return res;
        }

        public override void SetData(Dictionary<string, string> data)
        {
            pressure_combo.ActiveId = data["030н00"][0].ToString();
            power_combo.ActiveId = data["030н00"][1].ToString();
            entry5.Text = data["030н01"];
            entry6.Text = data["030н02"];
            entry7.Text = data["024"];
            entry8.Text = data["025"];
            entry9.Text = data["008"];
            spec1.Text = data["003"];
            spec2.Text = data["004"];
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

            power_combo.Changed += OnBlockChanged;
            pressure_combo.Changed += OnBlockChanged;
            entry5.Changed += OnBlockChanged;
            entry6.Changed += OnBlockChanged;
            entry7.Changed += OnBlockChanged;
            entry8.Changed += OnBlockChanged;
            entry9.Changed += OnBlockChanged;
            spec1.Changed += OnBlockChanged;
            spec2.Changed += OnBlockChanged;
            check5.Clicked += OnBlockChanged;
            //DeleteEvent += OnLocalDeleteEvent;
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

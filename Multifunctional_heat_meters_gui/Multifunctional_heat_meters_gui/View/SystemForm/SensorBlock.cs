using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class SensorBlock : WindowBlock
    {
        private Builder _builder;
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
        private Label measure_label2;


        public static SensorBlock Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SensorBlock.glade", null);
            return new SensorBlock(builder, builder.GetObject("box").Handle);
        }
        protected SensorBlock(Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            ShowAll();
            SetupHandlers();
        }

        public List<int> GetSensorsState()
        {
            List<int> result = new List<int>();
            result.Add(sensor_check1.Active ? 1 : 0);
            result.Add(sensor_check2.Active ? 1 : 0);
            result.Add(sensor_check3.Active ? 1 : 0);
            result.Add(sensor_check4.Active ? 1 : 0);
            return result;
        }

        public override Dictionary<string, string> GetResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "035н00", const_entry1.Text },
                //{ "035н01", sensor_check1.Active ? "1" : "0"},
                { "036н00", const_entry2.Text},
                //{ "036н01", sensor_check2.Active ? "1" : "0" },
                { "037н00", const_entry3.Text},
                //{ "037н01", sensor_check3.Active ? "1" : "0"},
                { "040н00", const_entry4.Text },
                //{ "040н01", sensor_check4.Active? "1" : "0"},
            };
            return res;
        }

        public override void SetData(Dictionary<string, string> data)
        {
            const_entry1.Text = data["035н00"];
            const_entry2.Text = data["036н00"];
            const_entry3.Text = data["037н00"];
            const_entry4.Text = data["040н00"];

            sensor_check1.Active = data["sensor1"] == "1" ? true: false;
            sensor_check2.Active = data["sensor2"] == "1" ? true : false;
            sensor_check3.Active = data["sensor3"] == "1" ? true : false;
            sensor_check4.Active = data["sensor4"] == "1" ? true : false;
        }

        public void ChangePressureMeasurement(int unitOfMeasurement)
        {
            switch (unitOfMeasurement)
            {
                case 0:
                    measure_label2.Text = "МПа";
                    break;
                case 1:
                    measure_label2.Text = "кгс/см2";
                    break;
            }
        }

        protected void SetupHandlers()
        {
            const_entry1.Changed += OnBlockChanged;
            const_entry2.Changed += OnBlockChanged;
            const_entry3.Changed += OnBlockChanged;
            const_entry4.Changed += OnBlockChanged;
            sensor_check1.Clicked += OnBlockChanged;
            sensor_check2.Clicked += OnBlockChanged;
            sensor_check3.Clicked += OnBlockChanged;
            sensor_check4.Clicked += OnBlockChanged;
            //DeleteEvent += OnLocalDeleteEvent;
        }
    }
}

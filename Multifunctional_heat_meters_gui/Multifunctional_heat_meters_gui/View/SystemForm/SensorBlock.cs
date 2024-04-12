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


        //private int _index;

        public static SensorBlock Create(string measureSystem)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.SensorBlock.glade", null);
            return new SensorBlock(measureSystem, builder, builder.GetObject("box").Handle);
        }
        protected SensorBlock(string measureSystem, Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            
            //_index = index;
            parameter_widget = new Dictionary<string, Entry>
            {
                { "035н00", const_entry1 },
                { "036н00", const_entry2 },
                { "037н00", const_entry3 },
                { "040н00", const_entry4 },
            };

            int measurement = Int32.Parse(measureSystem[0].ToString());
            ChangePressureMeasurement(measurement);

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
                { "035н00", parameter_widget["035н00"].Text },
                //{ "035н01", sensor_check1.Active ? "1" : "0"},
                { "036н00", parameter_widget["036н00"].Text},
                //{ "036н01", sensor_check2.Active ? "1" : "0" },
                { "037н00", parameter_widget["037н00"].Text},
                //{ "037н01", sensor_check3.Active ? "1" : "0"},
                { "040н00", parameter_widget["040н00"].Text },
                //{ "040н01", sensor_check4.Active? "1" : "0"},
            };
            return res;
        }

        public override void SetData(Dictionary<string, string> data)
        {
            parameter_widget["035н00"].Text = data["035н00"];
            parameter_widget["036н00"].Text = data["036н00"];
            parameter_widget["037н00"].Text = data["037н00"];
            parameter_widget["040н00"].Text = data["040н00"];

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
                    if(const_entry2.Text == "1")
                        const_entry2.Text = "0.1";
                    break;
                case 1:
                    measure_label2.Text = "кгс/см2";
                    if (const_entry2.Text == "0.1")
                        const_entry2.Text = "1";
                    break;
            }
        }

        /*public void SetAutoValueCheck(bool flag)
        {

        }*/

        protected void SetupHandlers()
        {
            const_entry1.Changed += TurnIntoNumber;
            const_entry2.Changed += TurnIntoNumber;
            const_entry3.Changed += TurnIntoNumber;
            const_entry4.Changed += TurnIntoNumber;

            //const_entry1.Changed += OnBlockChanged;
            foreach(KeyValuePair<string, Entry> keyval in parameter_widget)
                parameter_widget[keyval.Key].Changed += (sender, e) => OnValueChanged(sender, new List<string> { keyval.Key, keyval.Value.Text });


            //const_entry2.Changed += OnBlockChanged;
            //const_entry3.Changed += OnBlockChanged;
            //const_entry4.Changed += OnBlockChanged;
            sensor_check1.Clicked += OnBlockChanged;
            sensor_check2.Clicked += OnBlockChanged;
            sensor_check3.Clicked += OnBlockChanged;
            sensor_check4.Clicked += OnBlockChanged;

            
            //DeleteEvent += OnLocalDeleteEvent;
        }
    }
}

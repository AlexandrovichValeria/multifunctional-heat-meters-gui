using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Pipeline : AbstractModel
    {
        private bool _active;

        //private Dictionary<string, Parameter> _parameters;

        //Является ли трубопровод активным
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Dictionary<string, Parameter> Parameters
        {
            get { return _parameters; }
        }

        public Pipeline(bool active = false)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _parameters.Add("032н00", new Parameter("032н00", "042", "", ""));
            _parameters.Add("032н01", new Parameter("032н01", "16.31", "кгс/см2", "pressure")); //?
            _parameters.Add("032н02", new Parameter("032н02", "0", "", ""));
            _parameters.Add("032н08", new Parameter("032н08", "0", "кг/см2", "pressure"));
            _parameters.Add("033н00", new Parameter("033н00", "043", "", ""));
            _parameters.Add("033н01", new Parameter("033н01", "180", "'C", "temperature"));
            _parameters.Add("033н02", new Parameter("033н02", "0", "'C", "temperature"));
            _parameters.Add("034н00", new Parameter("034н00", "010", "", ""));
            _parameters.Add("034н01", new Parameter("034н01", "763.4", "м3/час", "time sensor dependant"));
            _parameters.Add("034н02", new Parameter("034н02", "0", "м3/час", "time sensor dependant"));
            _parameters.Add("034н06", new Parameter("034н06", "1000", "Гц", "")); //Только для "Частота вода"
            _parameters.Add("034н07", new Parameter("034н07", "0", "Гц", ""));    //Только для "Частота вода"
            _parameters.Add("034н08", new Parameter("034н08", "", "м3/имп", "impulse sensor dependant")); // Все, кроме "Частота вода"
            //Параметр 100 будет вноситься автоматически во время загрузки бд в файл(т.к. это номер трубопровода)
            _parameters.Add("101", new Parameter("101", "0", "", ""));
            _parameters.Add("102н00", new Parameter("102н00", "12", "", ""));
            _parameters.Add("102н03", new Parameter("102н03", "1", "", ""));
            _parameters.Add("104", new Parameter("104", "0", "", "pressure")); // Только для "Имп пар"
            _parameters.Add("105", new Parameter("105", "1", "", "")); // Только для "Имп пар"
            _parameters.Add("109н00", new Parameter("109н00", "0", "м3/час", "time sensor dependant"));
            _parameters.Add("109н01", new Parameter("109н01", "03401", "", ""));
            _parameters.Add("113н00", new Parameter("113н00", "4.5", "кгс/см2", "pressure"));
            _parameters.Add("113н01", new Parameter("113н01", "03202", "", ""));
            _parameters.Add("114н00", new Parameter("114н00", "70", "'C", "temperature"));
            _parameters.Add("114н01", new Parameter("114н01", "03302", "", ""));
            _parameters.Add("115н00", new Parameter("115н00", "11", "", ""));
            _parameters.Add("115н01", new Parameter("115н01", "0", "м3/ч", ""));
            _parameters.Add("120", new Parameter("120", "0", "т/час", ""));
            _parameters.Add("125н00", new Parameter("125н00", "", "'C", ""));
            _parameters.Add("125н01", new Parameter("125н01", "", "'C", ""));
            _parameters.Add("125н02", new Parameter("125н02", "", "кг/м3", ""));
            _parameters.Add("125н03", new Parameter("125н03", "", "кг/м3", ""));
            _parameters.Add("125н04", new Parameter("125н04", "", "кДж/кг", ""));
            _parameters.Add("125н05", new Parameter("125н05", "", "кДж/кг", ""));
            _parameters.Add("125н06", new Parameter("125н06", "", "мкПа*с", ""));
            _parameters.Add("125н07", new Parameter("125н07", "", "мкПа*с", ""));
        }


        /*public void ChangePowerMeasurement(int unitOfMeasurement)
        {
            
        }

        public void ChangePressureMeasurement(int unitOfMeasurement)
        {
            string pressure = "";
            string pressure_change = "";
            if (unitOfMeasurement == 0)
            {
                pressure = "МПа";
                pressure_change = "кПа";
            }
            else if (unitOfMeasurement == 1)
            {
                pressure = "кгс/см2";
                pressure_change = "кгс/м2";
            }
            foreach (KeyValuePair<string, Parameter> param in _parameters)
            {
                if (param.Value.TypeOfMeasurement == "pressure")
                    param.Value.UnitOfMeasurement = pressure;
                if (param.Value.TypeOfMeasurement == "pressure change")
                    param.Value.UnitOfMeasurement = pressure_change;
                /*if (param.Value.TypeOfMeasurement == "pressure")
                {
                    switch (unitOfMeasurement)
                    {
                        case 0:
                            param.Value.UnitOfMeasurement = "МПа";
                            break;
                        case 1:
                            param.Value.UnitOfMeasurement = "кгс/см2";
                            break;
                    }
                }
                else if (param.Value.TypeOfMeasurement == "pressure change")
                {
                    switch (unitOfMeasurement)
                    {
                        case 0:
                            param.Value.UnitOfMeasurement = "кПа";
                            break;
                        case 1:
                            param.Value.UnitOfMeasurement = "кгс/м2";
                            break;
                    }
                }
            }
        }

        public void ChangeSensorType(string sensorType)
        {
            string time_sensor = "";
            string impulse_sensor = "";
            if (sensorType == "01" || sensorType == "03")
            {
                time_sensor = "м3/час";
                impulse_sensor = "м3/имп";
            }
            else if (sensorType == "02" || sensorType == "04")
            {
                time_sensor = "т/час";
                impulse_sensor = "т/имп";
            }

            foreach (KeyValuePair<string, Parameter> param in _parameters)
            {
                if (param.Value.TypeOfMeasurement == "time sensor dependant")
                    param.Value.UnitOfMeasurement = time_sensor;
                if (param.Value.TypeOfMeasurement == "impulse sensor dependant")
                    param.Value.UnitOfMeasurement = impulse_sensor;
            }
        }*/

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class PressureSensor : Sensor
    {
        public PressureSensor(int type, bool active = false)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _type = type;
            _channel_number = 0;
            /*if (_type == 2)
            {
                _parameters.Add("036н01", new Parameter("036н01", "0", "", ""));
            }
            else if (_type == 3)
            {
                _parameters.Add("037н01", new Parameter("037н01", "0", "", ""));
            }*/
            //_parameters.Add("113н01", new Parameter("113н01", "03201", "", ""));
            _parameters.Add("032н00", new Parameter("032н00", "042", "", ""));
            _parameters.Add("032н01", new Parameter("032н01", "16.31", "кгс/см2", "pressure"));
            _parameters.Add("032н02", new Parameter("032н02", "0", "", ""));
            _parameters.Add("032н08", new Parameter("032н08", "0", "кгс/см2", "pressure"));
        }

        public override void ChangePowerMeasurement(int unitOfMeasurement)
        {
            foreach (KeyValuePair<string, Parameter> param in _parameters)
            {
                if (param.Value.TypeOfMeasurement == "energy")
                {
                    switch (unitOfMeasurement)
                    {
                        case 0:
                            param.Value.UnitOfMeasurement = "ГДж";
                            break;
                        case 1:
                            param.Value.UnitOfMeasurement = "Гкал";
                            break;
                        case 2:
                            param.Value.UnitOfMeasurement = "МВт*ч";
                            break;
                    }
                }
                else if (param.Value.TypeOfMeasurement == "power")
                {
                    switch (unitOfMeasurement)
                    {
                        case 0:
                            param.Value.UnitOfMeasurement = "ГДж";
                            break;
                        case 1:
                            param.Value.UnitOfMeasurement = "Гкал";
                            break;
                        case 2:
                            param.Value.UnitOfMeasurement = "МВт*ч";
                            break;
                    }
                }
            }
        }

        public override void ChangePressureMeasurement(int unitOfMeasurement)
        {
            foreach (KeyValuePair<string, Parameter> param in _parameters)
            {
                if (param.Value.TypeOfMeasurement == "pressure")
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
    }
}

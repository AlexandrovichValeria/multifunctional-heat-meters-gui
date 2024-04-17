using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class AbstractModel
    {
        protected Dictionary<string, Parameter> _parameters;

        public void ChangeParameterValue(string parameterName, string value)
        {
            _parameters[parameterName].Value = value;
        }

        public void ChangeParameterUnitOfMeasurement(string parameterName, string unitOfMeasurement)
        {
            _parameters[parameterName].UnitOfMeasurement = unitOfMeasurement;
        }

        public void ChangePowerMeasurement(int unitOfMeasurement)
        {
            string energy = "";
            string power = "";
            if (unitOfMeasurement == 0)
            {
                energy = "ГДж";
                power = "ГДж/ч";
            }
            else if (unitOfMeasurement == 1)
            {
                energy = "Гкал";
                power = "Гкал/ч";
            }
            else if (unitOfMeasurement == 2)
            {
                energy = "МВт*ч";
                power = "МВт";
            }
            foreach (KeyValuePair<string, Parameter> param in _parameters)
            {
                if (param.Value.TypeOfMeasurement == "energy")
                    param.Value.UnitOfMeasurement = energy;
                if (param.Value.TypeOfMeasurement == "power")
                    param.Value.UnitOfMeasurement = power;
            }
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
        }
    }
}

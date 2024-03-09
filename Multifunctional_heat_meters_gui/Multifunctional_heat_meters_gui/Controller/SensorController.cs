using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    class SensorController : Controller
    {
        private View.Sensor _view;

        private Model.Model _model;

        private Model.Sensor _sensorModel;

        public SensorController(View.Sensor view, Model.Model model, string name)
        {
            _view = view;
            _model = model;
            _sensorModel = model.GetSensorByName(name);
        }
        public override void SaveDataToModel()
        {
            Dictionary<string, string> currentData = _view.GetSensorSettings();

            foreach (KeyValuePair<string, string> paramValuePair in currentData)
            {
                if (paramValuePair.Value != "")
                {
                    _sensorModel.ChangeParameterValue(paramValuePair.Key, paramValuePair.Value);
                }
            }
        }
        public override void ChangePowerSystem(int typeOfMeasurement)
        {
            //update model
            _sensorModel.ChangePowerMeasurement(typeOfMeasurement);

            //update view
            _view.ChangePowerMeasurement(typeOfMeasurement);
        }

        public override void ChangePressureSystem(int typeOfMeasurement)
        {
            //Console.WriteLine("ChangePressureSystem");
            //update model
            _sensorModel.ChangePressureMeasurement(typeOfMeasurement);

            //update view
            _view.ChangePressureMeasurement(typeOfMeasurement);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    class PipelineController1 : Controller
    {
        private View.PipelineSettings1Form _view;

        private Model.Model _model;

        private Model.Pipeline _pipelineModel;

        public PipelineController1(View.PipelineSettings1Form view, Model.Model model, int index)
        {
            _view = view;
            _model = model;
            _pipelineModel = model.GetPipelineByInd(index);
            _index = index + 1;
        }
        public override void SaveDataToModel()
        {
            Dictionary<string, string> currentData = _view.GetPipelineSettings1();
            foreach (KeyValuePair<string, string> paramValuePair in currentData)
            {
                if (paramValuePair.Value != "")
                {
                    _pipelineModel.ChangeParameterValue(paramValuePair.Key, paramValuePair.Value);
                }
            }
        }

        public override void ChangePowerSystem(int typeOfMeasurement)
        {
            //update model
            _pipelineModel.ChangePowerMeasurement(typeOfMeasurement);

            //update view
            _view.ChangePowerMeasurement(typeOfMeasurement);
        }
        public override void ChangePressureSystem(int typeOfMeasurement)
        {
            //update model
            _pipelineModel.ChangePressureMeasurement(typeOfMeasurement);

            //update view
            _view.ChangePressureMeasurement(typeOfMeasurement);
        }
        public void ChangeSensorType(string sensorType)
        {
            //update model
            _pipelineModel.ChangeSensorType(sensorType);

            //update view
            _view.ChangeSensorType(sensorType);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    public class PipelineController2 : Controller
    {
        private View.PipelineSettings2Form _view;

        private Model.Model _model;

        private Model.Pipeline _pipelineModel;

        public PipelineController2(View.PipelineSettings2Form view, Model.Model model, int index)
        {
            _view = view;
            _model = model;
            _pipelineModel = model.GetPipelineByInd(index);
            _index = index + 1;
        }
        public override void SaveDataToModel()
        {
            Dictionary<string, string> currentData = _view.GetPipelineSettings2();

            foreach (KeyValuePair<string, string> paramValuePair in currentData)
            {
                if (paramValuePair.Value != "")
                {
                    _pipelineModel.ChangeParameterValue(paramValuePair.Key, paramValuePair.Value);
                }
            }
        }

        public void ChangeSensorType(string sensorType)
        {
            //update model
            _pipelineModel.ChangeSensorType(sensorType);

            //update view
            _view.ChangeSensorType(sensorType);
        }

        public void ChangeLowerLimit(string lowerLimit)
        {
            //update view
            _view.ChangeLowerLimit(lowerLimit);
        }
        
    }
}

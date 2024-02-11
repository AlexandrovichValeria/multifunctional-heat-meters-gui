using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    public class SystemController : Controller
    {

        private readonly View.SystemForm _view;
        private readonly Model.Model _model;
        private readonly Model.SystemWideSettings _systemModel;

        public SystemController(View.SystemForm view, Model.Model model)
        {
            _view = view;
            _model = model;
            _systemModel = model.SystemWideSettings;

        }

        private void ActivatePipelines(string participatedPipelinesString)
        {
            for (int ind = 0; ind < participatedPipelinesString.Length; ind++)
            {
                if (participatedPipelinesString[ind] == '1')
                {
                    _model.GetPipelineByInd(ind).Active = true;
                }
            }
        }

        private void ActivateConsumers(string participatedConsumersString)
        {
            for (int ind = 0; ind < participatedConsumersString.Length; ind++)
            {
                if (participatedConsumersString[ind] == '1')
                {
                    _model.GetConsumerByInd(ind).Active = true;
                }
            }
        }

        public override void SaveDataToModel()
        {
            Dictionary<string, string> currentData = _view.GetSystemWindowData();


            if (currentData["CurrentTimeAndDate"] == "1")
            {
                _systemModel.ChangeParameterValue("020", DateTime.Now.Date.ToString("dd-MM-yy"));
                _systemModel.ChangeParameterValue("021", DateTime.Now.ToString("HH-mm-ss"));
            }

            currentData.Remove("CurrentTimeAndDate");

            ActivatePipelines(currentData["031н00"]);
            ActivateConsumers(currentData["031н01"]);

            foreach (KeyValuePair<string, string> paramValuePair in currentData)
            {
                if (paramValuePair.Value != "")
                {
                    _systemModel.ChangeParameterValue(paramValuePair.Key, paramValuePair.Value);
                }
            }
        }

    }
}

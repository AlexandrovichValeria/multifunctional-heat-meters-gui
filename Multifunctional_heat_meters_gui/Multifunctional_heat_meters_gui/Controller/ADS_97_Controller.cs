using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    public class ADS_97_Controller : Controller
    {
        private View.ADS_97_Form _view;

        private Model.SystemWideSettings _systemModel;

        public ADS_97_Controller(View.ADS_97_Form view, Model.Model model)
        {
            _view = view;
            _systemModel = model.SystemWideSettings;
        }

        public override void SaveDataToModel()
        {
            Console.WriteLine("ADS_97_Controller");
            Dictionary<string, string> result = _view.GetADS_97_results();
            foreach(KeyValuePair<string, string> keyval in result)
            {
                _systemModel.ChangeParameterValue(keyval.Key, keyval.Value);
            }
        }
    }
}
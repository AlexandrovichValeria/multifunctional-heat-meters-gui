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
            /*_systemModel.ChangeParameterValue("038н00", _view.AdaptersCountCombobox.Text);
            _systemModel.ChangeParameterValue("038н01", _view.FirstAdapterNumber.Value.ToString());
            if (_view.AdaptersCountCombobox.SelectedIndex == 1)
                _systemModel.ChangeParameterValue("038н02", _view.SecondAdapterNumber.Value.ToString());*/
        }
    }
}
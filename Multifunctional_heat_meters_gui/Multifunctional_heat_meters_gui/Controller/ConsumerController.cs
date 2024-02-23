using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.Controller
{
    public class ConsumerController : Controller
    {
        private View.ConsumerForm _view;

        private Model.Model _model;

        private Model.Consumer _consumerModel;

        public ConsumerController(View.ConsumerForm view, Model.Model model, int index)
        {
            _view = view;
            _model = model;
            _consumerModel = model.GetConsumerByInd(index);
        }

        public override void SaveDataToModel()
        {
            _consumerModel.Id = (int)_view.spinbutton1.Value;
            _consumerModel.AccountingSchemeNumber = Int32.Parse(_view.combo1.ActiveId);
            int pipelinesCount = _model.SystemWideSettings.PipelinesCount;
            for (int i = 0; i < pipelinesCount; i++)
            {
                if (_model.GetPipelineByInd(i).Active == false)
                    continue;
                ComboBoxText comboBox = (ComboBoxText)_view._builder.GetObject("pipeline_combo" + (i + 1).ToString());
                _consumerModel.SetPipelineStatusByInd(i, (Model.Consumer.PipelineStatus)Enum.Parse(typeof(Model.Consumer.PipelineStatus), comboBox.ActiveId));
            }
        }
    }
}

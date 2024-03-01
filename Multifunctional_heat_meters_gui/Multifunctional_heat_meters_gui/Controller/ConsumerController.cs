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
            Dictionary<string, string> data = _view.GetConsumerSettings();

            _consumerModel.Id = Int32.Parse(data["consumer_id"]);
            _consumerModel.AccountingSchemeNumber = Int32.Parse(data["accountingSchemeNumber"]);
            //_consumerModel.Id = (int)_view.spin_button1.Value;
            //_consumerModel.AccountingSchemeNumber = _view.combo1.Active;
            int pipelinesCount = _model.SystemWideSettings.PipelinesCount;
            for (int i = 0; i < pipelinesCount; i++) //для всех чисел
            {
                
                if (_model.GetPipelineByInd(i).Active == false) // пропускаем неактивные
                    continue;
                _consumerModel.SetPipelineStatusByInd(i, (Model.Consumer.PipelineStatus)Enum.Parse(typeof(Model.Consumer.PipelineStatus), data[(i+1).ToString()])); //забираем у него значение

                //ComboBoxText comboBox = (ComboBoxText)_view._builder.GetObject("pipeline_combo" + (i + 1).ToString()); //находим комбо по id (номер трубопровода)
                //_consumerModel.SetPipelineStatusByInd(i, (Model.Consumer.PipelineStatus)Enum.Parse(typeof(Model.Consumer.PipelineStatus), comboBox.ActiveId)); //забираем у него значение
            }
        }
    }
}

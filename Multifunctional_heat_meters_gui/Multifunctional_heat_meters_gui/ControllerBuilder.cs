using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Multifunctional_heat_meters_gui
{
    public class ControllerBuilder
    {
        private LinkedList<Controller.Controller> _controllers;
        private AppState _appState;
        private Model.Model _model;
        private Controller.SystemController _sysController;
        private View.SystemForm _sysForm;

        public ControllerBuilder(AppState appstate, Model.Model model /*Controller.Controller controllers*/)
        {
            _controllers = new LinkedList<Controller.Controller>();
            _appState = appstate;
            _model = model;

            foreach (var form in _appState.GetForms())
            {
                Controller.Controller controller = null;
                if (form.FormName.StartsWith("Общесистемные"))
                {
                    controller = new Controller.SystemController((View.SystemForm)form, _model);
                }
                else if (form.FormName.StartsWith("Потребитель"))
                {
                    controller = new Controller.ConsumerController((View.ConsumerForm)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                else if (form.FormName.StartsWith("Теплоноситель"))
                {
                    controller = new Controller.CoolantController((View.CoolantSelectionForm)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                else if (form.FormName.StartsWith("Первая настройка трубопровода"))
                {
                    controller = new Controller.PipelineController1((View.PipelineSettings1Form)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                else if (form.FormName.StartsWith("Вторая настройка трубопровода"))
                {
                    controller = new Controller.PipelineController2((View.PipelineSettings2Form)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                if (controller != null)
                    _controllers.AddLast(controller);
            }
            _sysController = (Controller.SystemController)_controllers.First();
            _sysForm = (View.SystemForm)_appState.GetForms().First();
            SetupHandlers();
        }

        public void saveDataFromAllForms()
        { 
            foreach(Controller.Controller controller in _controllers)
            {
                controller.SaveDataToModel();
            }
        }

        protected void SetupHandlers()
        {
            _sysForm.PowerSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(ChangePowerSystem);
            _sysForm.PressureSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(ChangePressureSystem);

        }
        public void ChangePowerSystem(object sender, EventsArgs.MeasurementEventArgs args)
        {
            foreach(Controller.Controller controller in _controllers)
            {
                controller.ChangePowerSystem(args.typeOfMeasurement);
            }
        }

        public void ChangePressureSystem(object sender, EventsArgs.MeasurementEventArgs args)
        {
            foreach (Controller.Controller controller in _controllers)
            {
                controller.ChangePressureSystem(args.typeOfMeasurement);
            }
        }
    }
}

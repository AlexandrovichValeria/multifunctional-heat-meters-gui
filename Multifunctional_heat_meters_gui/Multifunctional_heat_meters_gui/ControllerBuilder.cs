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

        public ControllerBuilder(AppState appstate, Model.Model model)
        {
            _controllers = new LinkedList<Controller.Controller>();
            _appState = appstate;
            _model = model;
            
            _sysForm = (View.SystemForm)_appState.GetForms().First();

            SetControllers();
            _sysController = (Controller.SystemController)_controllers.First();
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
            //_sysForm.SystemFormChangedEvent += new EventHandler(ChangePressureSystem);
        }

        /*public void DeleteControllerForm(object form, EventArgs args)
        {
            View.WindowForm _form = (View.WindowForm)form;
        }*/

        public void SetControllers()
        {
            int typeOfPowerMeasurement = _sysForm.GetMeasurementSystems()["Power"];
            int typeOfPressureMeasurement = _sysForm.GetMeasurementSystems()["Pressure"];

            foreach (var form in _appState.GetForms())
            {
                Controller.Controller controller = null;
                if (form.FormName.StartsWith("Общесистемные"))
                {
                    controller = new Controller.SystemController((View.SystemForm)form, _model);
                }
                else if (form.FormName.StartsWith("п"))
                {
                    controller = new Controller.ConsumerController((View.ConsumerForm)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                else if (form.FormName.StartsWith("Теплоноситель"))
                {
                    controller = new Controller.CoolantController((View.CoolantSelectionForm)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                    View.CoolantSelectionForm coolantForm = (View.CoolantSelectionForm)form;
                    coolantForm.SensorTypeChangedEvent += new EventHandler<EventsArgs.SensorTypeEventArgs>(ChangeSensorType);
                }
                else if (form.FormName.StartsWith("Первая настройка трубопровода"))
                {
                    controller = new Controller.PipelineController1((View.PipelineSettings1Form)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                    controller.ChangePressureSystem(typeOfPressureMeasurement);
                    controller.ChangePowerSystem(typeOfPowerMeasurement);
                }
                else if (form.FormName.StartsWith("Вторая настройка трубопровода"))
                {
                    controller = new Controller.PipelineController2((View.PipelineSettings2Form)form, _model, int.Parse(Regex.Match(form.FormName, @"\d+$").Value) - 1);
                }
                else if (form.FormName.StartsWith("Датчик"))
                {
                    controller = new Controller.SensorController((View.Sensor)form, _model, form.FormName);
                    controller.ChangePressureSystem(typeOfPressureMeasurement);
                    controller.ChangePowerSystem(typeOfPowerMeasurement);
                }

                if (controller != null)
                {
                    _controllers.AddLast(controller);
                }
            }
        }

        public void ResetControllers(object sender, EventsArgs.MenuEventArgs args)
        {
            _controllers.Clear();
            SetControllers();
        }

        public void ChangePowerSystem(object sender, EventsArgs.MeasurementEventArgs args)
        {
            foreach (Controller.Controller controller in _controllers)
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

        public void ChangeSensorType(object sender, EventsArgs.SensorTypeEventArgs args)
        {
            foreach (Controller.Controller controller in _controllers)
            {
                if (controller is Controller.PipelineController1 && controller._index == args.PipelineIndex)
                {
                    controller.ChangeSensorType(args.SensorType);
                }
            }
        }
    }
}

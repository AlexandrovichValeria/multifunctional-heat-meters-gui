using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui
{
    public class FormsBuilder
    {

        public event EventHandler NewFormCreatedEvent;
        public event EventHandler FormDeletedEvent;
        public event EventHandler<EventsArgs.MenuEventArgs> MenuShouldBeUpdatedEvent;

        public enum FormsName
        {
            CONSUMERS,
            PIPELINES,
            SENSORS
        }

        private LinkedList<View.WindowForm> _forms;
        private View.SystemForm _systemForm;
        private View.SystemForm _sensorForm;

        private List<int> _currentPipelinesNumbers = new List<int>();
        private List<int> _currentConsumersNumbers = new List<int>();
        private List<int> _currentSensorsNumbers = new List<int>();

        private static readonly string pipelinesParam = "031н00";
        private static readonly string consumersParam = "031н01";

        private static readonly Dictionary<string, string> sensors = new Dictionary<string, string>()
        {
            { "ColdWaterTemperature", "035н01" },
            { "ColdWaterPressure", "036н01" },
            { "BarometricPressure", "037н01" },
            { "AirTemperature", "040н01" },
        };

        public FormsBuilder(LinkedList<View.WindowForm> forms)
        {
            _forms = forms;

            View.WindowForm systemWindow1 = GetFormByName("Общесистемные параметры 1");

            if (systemWindow1 != null)
            {
                _systemForm = systemWindow1 as View.SystemForm;
                _systemForm.NextFormEvent += new EventHandler<EventsArgs.NextFormArgs>(SystemWindowParamsSet);
            }

            View.WindowForm systemWindow2 = GetFormByName("Общесистемные параметры 2");

            if (systemWindow2 != null)
            {
                _sensorForm = systemWindow2 as View.SystemForm;
                _sensorForm.NextFormEvent += new EventHandler<EventsArgs.NextFormArgs>(SystemWindowParamsSet);
            }

        }

        private View.WindowForm GetFormByName(string name)
        {

            LinkedListNode<View.WindowForm> currentNode = _forms.First;

            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;

                if (currentForm.FormName == name)
                {
                    return currentForm;
                }

                currentNode = currentNode.Next;
            }

            return null;
        }

        private void SystemWindowParamsSet(object sender, EventsArgs.NextFormArgs e)
        {
            string zeroOneStringConsumers = _systemForm.GetParamFromWindow(consumersParam);
            string zeroOneStringPipelines = _systemForm.GetParamFromWindow(pipelinesParam);

            List<int> nextConsumersNumbers = GetNumbersOfOneFromZeroOneString(zeroOneStringConsumers);
            List<int> nextPipelinesNumbers = GetNumbersOfOneFromZeroOneString(zeroOneStringPipelines);
            List<int> nextSensorsNumbers = GetSensorNumbers();

            List<int> consumersNumbersToAdd = GetFormsNumbersToAdd(_currentConsumersNumbers, nextConsumersNumbers);
            List<int> pipelinesNumbersToAdd = GetFormsNumbersToAdd(_currentPipelinesNumbers, nextPipelinesNumbers);
            List<int> sensorsNumbersToAdd = GetFormsNumbersToAdd(_currentSensorsNumbers, nextSensorsNumbers);

            List<int> consumersNumbersToDelete = GetFormsNumbersToDelete(_currentConsumersNumbers, nextConsumersNumbers);
            List<int> pipelinesNumbersToDelete = GetFormsNumbersToDelete(_currentPipelinesNumbers, nextPipelinesNumbers);
            List<int> sensorsNumbersToDelete = GetFormsNumbersToDelete(_currentSensorsNumbers, nextSensorsNumbers);

            /*string coldWaterTemperatureSensor = _sensorForm.GetParamFromWindow(sensors["ColdWaterTemperature"]);
            string coldWaterPressureSensor = _sensorForm.GetParamFromWindow(sensors["ColdWaterPressure"]);
            string barometricPressureSensor = _sensorForm.GetParamFromWindow(sensors["BarometricPressure"]);
            string airTemperatureSensor = _sensorForm.GetParamFromWindow(sensors["AirTemperature"]);*/


            if (pipelinesNumbersToAdd.Count == 0 & pipelinesNumbersToDelete.Count == 0)
            {
                if (consumersNumbersToAdd.Count != 0 | consumersNumbersToDelete.Count != 0)
                {
                    DeleteFormsByFormsNumbers<View.ConsumerForm>(consumersNumbersToDelete);
                    CreateConsumerWindows(consumersNumbersToAdd, _currentPipelinesNumbers);
                }
            }
            else
            {
                DeletePipelinesSettings(pipelinesNumbersToDelete);
                CreatePipelinesWindows(pipelinesNumbersToAdd);
                DeleteFormsByFormsNumbers<View.ConsumerForm>(_currentConsumersNumbers);
                CreateConsumerWindows(nextConsumersNumbers, nextPipelinesNumbers);
            }
            DeleteFormsByFormsNumbers<View.Sensor>(_currentSensorsNumbers);
            CreateSensorWindows(sensorsNumbersToAdd);

            if (pipelinesNumbersToAdd.Count != 0 | pipelinesNumbersToDelete.Count != 0 | consumersNumbersToAdd.Count != 0 | consumersNumbersToDelete.Count != 0 | sensorsNumbersToAdd.Count != 0 | sensorsNumbersToDelete.Count != 0)
            {
                EventsArgs.MenuEventArgs consumersArgs = new EventsArgs.MenuEventArgs(View.ContentMenu.DeepButtonsNames.CONSUMERS, nextConsumersNumbers);
                MenuShouldBeUpdatedEvent?.Invoke(this, consumersArgs);

                EventsArgs.MenuEventArgs pipelinesArgs = new EventsArgs.MenuEventArgs(View.ContentMenu.DeepButtonsNames.PIPELINES, nextPipelinesNumbers);
                MenuShouldBeUpdatedEvent?.Invoke(this, pipelinesArgs);

                EventsArgs.MenuEventArgs sensorsArgs = new EventsArgs.MenuEventArgs(View.ContentMenu.DeepButtonsNames.SENSORS, nextSensorsNumbers);
                MenuShouldBeUpdatedEvent?.Invoke(this, sensorsArgs);
            }

            _currentConsumersNumbers = nextConsumersNumbers;
            _currentPipelinesNumbers = nextPipelinesNumbers;

            //delete sensor forms
            /*if(coldWaterTemperatureSensor == "1")
            {
                Console.WriteLine("coldWaterTemperatureSensor == 1");
                View.TempSensor coldWaterTempWindow = View.TempSensor.Create("Холодная вода");
                InsertNewSensor(coldWaterTempWindow);
                NewFormCreatedEvent?.Invoke(coldWaterTempWindow, EventArgs.Empty);

                EventsArgs.MenuEventArgs sensorsArgs = new EventsArgs.MenuEventArgs(View.ContentMenu.DeepButtonsNames.SENSORS, sensorsToAdd);
                MenuShouldBeUpdatedEvent?.Invoke(this, sensorsArgs);
            }*/
            
        }

        private List<int> GetSensorNumbers()
        {
            /*List<string> sensorsIncludedOrNot = new List<string>{
                _sensorForm.GetParamFromWindow(sensors["ColdWaterTemperature"]),
                _sensorForm.GetParamFromWindow(sensors["ColdWaterPressure"]),
                _sensorForm.GetParamFromWindow(sensors["BarometricPressure"]),
                _sensorForm.GetParamFromWindow(sensors["AirTemperature"]) };*/
            List<int> sensorsIncludedOrNot = _sensorForm.GetSensorsState();
            List<int> result = new List<int>();
            for(int i = 0; i < sensorsIncludedOrNot.Count; i++)
            {
                if (sensorsIncludedOrNot[i] == 1)
                {
                    result.Add(i+1);
                }
            }
            //delete barometric sensor
            result.RemoveAll(p => p == 3);
            return result;
        }

        private void InsertNewCustomer(View.ConsumerForm form)
        {
            LinkedListNode<View.WindowForm> beforeNode = GetBeforeNodeForNumber<View.ConsumerForm>(form.FormIndex);

            if (beforeNode != null)
            {
                _forms.AddAfter(beforeNode, form);
            }
            else
            {
                _forms.AddAfter(_forms.Last, form);
            }
        }
        
        private void InsertNewSensor(View.Sensor form)
        {
            LinkedListNode<View.WindowForm> currentNode = _forms.First;
            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;
                if (currentForm.FormName == "Общесистемные параметры 2")
                {
                    break;
                }
                currentNode = currentNode.Next;
            }
            if (currentNode != null)
            {
                _forms.AddAfter(currentNode, form);
            }
        }
        
        private void InsertFormsListInLinkedList(LinkedListNode<View.WindowForm> beforeNode, List<View.WindowForm> forms)
        {
            LinkedListNode<View.WindowForm> currentBeforeNode = beforeNode;

            foreach (View.WindowForm form in forms)
            {
                _forms.AddAfter(currentBeforeNode, form);
                currentBeforeNode = currentBeforeNode.Next;
            }
        }

        private void InsertNewPipelinesSettings(List<View.WindowForm> forms)
        {
            LinkedListNode<View.WindowForm> beforeNode = GetBeforeNodeForNumber<View.PipelineSettings2Form>(forms[0].FormIndex);
            
            if (beforeNode != null)
            {
                InsertFormsListInLinkedList(beforeNode, forms);
            }
            else
            {
                InsertFormsListInLinkedList(_forms.First, forms);
            }
        }

        private LinkedListNode<View.WindowForm> GetBeforeNodeForNumber<T>(int number)
        {
            LinkedListNode<View.WindowForm> currentNode = _forms.Last;

            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;

                if (currentForm.FormIndex < number & currentForm is T)
                {
                    return currentNode;
                }

                currentNode = currentNode.Previous;
            }
            return null;
        }


        private List<int> GetFormsNumbersToAdd(List<int> currentNumbers, List<int> nextNumbers)
        {
            return nextNumbers.Where((int number) => !currentNumbers.Contains(number)).ToList();
        }

        private List<int> GetFormsNumbersToDelete(List<int> previousNumbers, List<int> currentNumbers)
        {
            return GetFormsNumbersToAdd(currentNumbers, previousNumbers);
        }

        private void DeletePipelinesSettings(List<int> formsNumbers)
        {
            DeleteFormsByFormsNumbers<View.CoolantSelectionForm>(formsNumbers);
            DeleteFormsByFormsNumbers<View.PipelineSettings1Form>(formsNumbers);
            DeleteFormsByFormsNumbers<View.PipelineSettings2Form>(formsNumbers);
        }


        private void DeleteFormsByFormsNumbers<FormType>(List<int> formsNumbers)
        {
            LinkedListNode<View.WindowForm> currentNode = _forms.Last;
            List<LinkedListNode<View.WindowForm>> nodesToDelete = new List<LinkedListNode<View.WindowForm>>();

            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;
                //FormDeletedEvent?.Invoke(currentForm, EventArgs.Empty);

                if (formsNumbers.Contains(currentForm.FormIndex) & currentForm is FormType)
                {
                    nodesToDelete.Add(currentNode);
                }

                currentNode = currentNode.Previous;
            }

            foreach (LinkedListNode<View.WindowForm> nodeToDelete in nodesToDelete)
            {
                _forms.Remove(nodeToDelete);
            }
        }


        private List<int> GetNumbersOfOneFromZeroOneString(string zeroOneString)
        {

            List<int> numbers = new List<int>();

            for (int i = 0; i < zeroOneString.Length; i++)
            {
                if (zeroOneString[i] == '1')
                {
                    numbers.Add(i + 1);
                }
            }

            return numbers;
        }

        private void CreateConsumerWindows(List<int> consumersNumbers, List<int> pipelinesNumbers)
        {
            foreach (int consumerNumber in consumersNumbers)
            {
                View.ConsumerForm newConsumerWindow = View.ConsumerForm.Create(pipelinesNumbers, consumerNumber);
                InsertNewCustomer(newConsumerWindow);
                NewFormCreatedEvent?.Invoke(newConsumerWindow, EventArgs.Empty);
            }
        }

        private void CreatePipelinesWindows(List<int> pipelinesNumbers)
        {

            foreach (int pipelineNumber in pipelinesNumbers)
            {
                View.CoolantSelectionForm coolantSelectionForm = View.CoolantSelectionForm.Create(pipelineNumber);
                View.PipelineSettings1Form pipelineSettings1Form = View.PipelineSettings1Form.Create(pipelineNumber);
                View.PipelineSettings2Form pipelineSettings2Form = View.PipelineSettings2Form.Create(pipelineNumber);

                //coolantSelectionForm.SetNextPipelineSettings(pipelineSettingsLimits);
                //pipelineSettingsLimits.SetNextPipelineSettings(pipelineSettings2Form);

                List<View.WindowForm> pipelinesSettingsForms = new List<View.WindowForm>() { coolantSelectionForm, pipelineSettings1Form, pipelineSettings2Form };

                InsertNewPipelinesSettings(pipelinesSettingsForms);

                NewFormCreatedEvent?.Invoke(coolantSelectionForm, EventArgs.Empty);
                NewFormCreatedEvent?.Invoke(pipelineSettings1Form, EventArgs.Empty);
                NewFormCreatedEvent?.Invoke(pipelineSettings2Form, EventArgs.Empty);
            }

            //EventsArgs.MenuEventArgs args = new EventsArgs.MenuEventArgs(View.ContentMenu.DeepButtonsNames.PIPELINES, pipelinesNumbers);
            //MenuShouldBeUpdatedEvent?.Invoke(this, args);
        }

        private void CreateSensorWindows(List<int> sensorNumbers)
        {
            Console.WriteLine("CreateSensorWindows");
            foreach (int sensorNumber in sensorNumbers.Reverse<int>())
            {
                if(sensorNumber == 1)
                {
                    View.TemperatureSensor coldWaterTempWindow = View.TemperatureSensor.Create("холодной воды");
                    InsertNewSensor(coldWaterTempWindow);
                    NewFormCreatedEvent?.Invoke(coldWaterTempWindow, EventArgs.Empty);
                    //Console.WriteLine(coldWaterTempWindow.FormName);
                }
                if (sensorNumber == 2)
                {
                    View.PressureSensor coldWaterpressureWindow = View.PressureSensor.Create("холодной воды");
                    InsertNewSensor(coldWaterpressureWindow);
                    NewFormCreatedEvent?.Invoke(coldWaterpressureWindow, EventArgs.Empty);
                    //Console.WriteLine(coldWaterpressureWindow.FormName);
                }
                if (sensorNumber == 4)
                {
                    View.TemperatureSensor AirTempWindow = View.TemperatureSensor.Create("наружного воздуха");
                    InsertNewSensor(AirTempWindow);
                    NewFormCreatedEvent?.Invoke(AirTempWindow, EventArgs.Empty);
                    //Console.WriteLine(coldWaterpressureWindow.FormName);
                }
            }
        }
    }
}

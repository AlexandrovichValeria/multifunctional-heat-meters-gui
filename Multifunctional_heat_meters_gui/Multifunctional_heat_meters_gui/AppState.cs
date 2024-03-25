using System.Collections.Generic;

namespace Multifunctional_heat_meters_gui
{
    public class AppState
    {

        private LinkedList<View.WindowForm> _forms;
        private View.ADS_97_Form _ADS_form;

        public AppState(LinkedList<View.WindowForm> forms, View.ADS_97_Form ADS_form)
        {
            _forms = forms;
            _ADS_form = ADS_form;
        }

        public LinkedList<View.WindowForm> GetForms()
        {
            return _forms;
        }

        public View.ADS_97_Form GetADSForm()
        {
            return _ADS_form;
        }

        public bool AreAllPipelinesFilledOut()
        {
            return AreAllWindowsFilledOut<View.CoolantSelectionForm>() & AreAllWindowsFilledOut<View.PipelineSettings2Form>() & AreAllWindowsFilledOut<View.PipelineSettings2Form>();
        }

        public bool AreAllWindowsFilledOut<Window>()
        {
            LinkedListNode<View.WindowForm> currentNode = _forms.First;

            int countWindows = 0;

            while (currentNode != null)
            {
                View.WindowForm currentForm = currentNode.Value;

                if (currentForm is Window)
                {
                    countWindows++;
                    if (!currentForm.IsFormFilledOut())
                    {
                        return false;
                    }
                }
                currentNode = currentNode.Next;
            }

            if (countWindows == 0)
            {
                return false;
            }

            return true;
        }
    }
}

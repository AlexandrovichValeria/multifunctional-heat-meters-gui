using System.Collections.Generic;

namespace Multifunctional_heat_meters_gui
{
    public class AppState
    {

        private LinkedList<View.WindowForm> _forms;

        public AppState(LinkedList<View.WindowForm> forms)
        {
            _forms = forms;
        }

        public LinkedList<View.WindowForm> GetForms()
        {
            return _forms;
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

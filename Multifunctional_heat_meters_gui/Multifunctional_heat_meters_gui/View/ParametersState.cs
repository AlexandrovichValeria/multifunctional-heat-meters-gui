using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.View
{
    public class ParametersState
    {
        private Dictionary<string, bool> _parametersState;

        public ParametersState(List<string> parameter_names)
        {
            _parametersState = new Dictionary<string, bool>();
            foreach(string param_name in parameter_names)
            {
                _parametersState.Add(param_name, true);
            }
        }

        public Dictionary<string, bool> GetState()
        {
            return _parametersState;
        }

        public void ChangeParameterState(string name, bool state)
        {
            _parametersState[name] = state;
        }
    }
}

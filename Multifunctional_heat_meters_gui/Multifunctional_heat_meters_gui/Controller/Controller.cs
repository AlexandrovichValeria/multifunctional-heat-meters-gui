using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Controller
{
    public abstract class Controller
    {
        public abstract void SaveDataToModel();

        public virtual void ChangePowerSystem(int typeOfMeasurement) { }

        public virtual void ChangePressureSystem(int typeOfMeasurement) { }

    }
}

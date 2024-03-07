using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui
{
    public class Dictionaries
    {
        public static readonly Dictionary<int, string> sensorNames = new Dictionary<int, string>()
        {
            {1, "Датчик температуры холодной воды" },
            {2, "Датчик давления холодной воды" },
            {3, "Датчик барометрического давления" },
            {4, "Датчик температуры наружного воздуха" },
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui
{
    public class Dictionaries
    {
        public static readonly Dictionary<int, string> sensorNames = new Dictionary<int, string>
        {
            {1, "Датчик температуры холодной воды" },
            {2, "Датчик давления холодной воды" },
            {3, "Датчик барометрического давления" },
            {4, "Датчик температуры наружного воздуха" },
        };

        public static readonly Dictionary<string, string> parameterPatterns = new Dictionary<string, string>
        {
            { "003", "^[12][0-4][0-9][0-3][01](0[0-9]|1[0-9]|2[0-9])(0[0-9]|1[0-9]|2[0-9])[1-9]$"},
            { "004", "^[12][0-4][0-9][0-3][0](0[0-9]|1[0-9]|2[0-9])(0[0-9]|1[0-9]|2[0-9])[1-9]$"},
            //{ "firstParameter", "^[01]+$" }, // String of zeros and ones
            //{ "secondParameter", "^[A-Za-z0-9]+$" } // Alphanumeric string
            // Add more parameters and patterns as needed
        };
    }
}

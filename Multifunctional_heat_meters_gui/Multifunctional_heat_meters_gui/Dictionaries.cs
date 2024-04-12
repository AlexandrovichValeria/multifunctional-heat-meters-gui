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

        public enum TopButtonsTypes
        {
            SYSTEM,
            PIPELINES,
            SENSORS,
            CONSUMERS
        }

        public static readonly Dictionary<TopButtonsTypes, string> topButtonNames = new Dictionary<TopButtonsTypes, string>()
        {
            { TopButtonsTypes.SYSTEM, "Общесистемные параметры" },
            { TopButtonsTypes.PIPELINES, "Настройка трубопроводов" },
            { TopButtonsTypes.SENSORS, "Настройка датчиков общесистемного канала" },
            { TopButtonsTypes.CONSUMERS, "Настройка потребителей" },
        };

        public static readonly Dictionary<string, string> parameterPatterns = new Dictionary<string, string>
        {
            { "003", "^[12][0-4][0-9][0-3][01](0[0-9]|1[0-9]|2[0-9])(0[0-9]|1[0-9]|2[0-9])[1-9]$"},
            { "004", "^[12][0-4][0-9][0-3][0](0[0-9]|1[0-9]|2[0-9])(0[0-9]|1[0-9]|2[0-9])[1-9]$"},
            { "030н01", "^(1|0\\.1|0\\.01|0\\.001|0\\.0001|0\\.00001|0\\.000001)$"},
            { "030н02", "^(1|0\\.1|0\\.01|0\\.001|0\\.0001|0\\.00001|0\\.000001)$"},
            { "024", "^(0[0-9]|1[0-9]|2[0-3])$"},
            { "008", "(^$)|(^\\d{13}$)"},
        };

        public static readonly Dictionary<string, List<double>> parameterDoubleLimits = new Dictionary<string, List<double>>
        {
            {"036н00", new List<double>{ 0.08, 160 } },
            {"040н00", new List<double>{ -50, 50 } },
            {"104", new List<double>{ 0, 1 } },
            {"105", new List<double>{ 0, 1 } },
            {"034н06", new List<double>{ 0, 5000 } },
            {"034н07", new List<double>{ 0, 5000 } },
            {"034н08", new List<double>{ 0, 100000 } },
        };

        public static readonly Dictionary<string, List<int>> parameterIntLimits = new Dictionary<string, List<int>>
        {
            { "025", new List<int>{ 1, 28 } },
            { "035н00", new List<int>{ 0, 100 } },
            { "037н00", new List<int>{ 500, 900 }},
            { "125н00", new List<int>{ -50, 200 }},
            { "125н01", new List<int>{ -50, 200 }},
            { "125н02", new List<int>{ 500, 1500 }},
            { "125н03", new List<int>{ 500, 1500 }},
            { "125н04", new List<int>{ 0, 1000 }},
            { "125н05", new List<int>{ 0, 1000 }},
            { "125н06", new List<int>{ 10, 2000 }},
            { "125н07", new List<int>{ 10, 2000 }},
            { "113н00", new List<int>{ 0, 300 }},
            { "033н01", new List<int>{ -50, 600 }},
            { "033н02", new List<int>{ -50, 100 }},
            { "120", new List<int>{ 0, 1000000 }},
            
        };

        public static readonly Dictionary<Model.Device, List<int>> MaxChannelAmountForDevice = new Dictionary<Model.Device, List<int>>
        {
            { Model.Device.SPT961, new List<int>{ 8, 4, 4 } },
            { Model.Device.SPT962, new List<int>{ 8, 4, 4 } },
            { Model.Device.SPT963, new List<int>{ 8, 8, 8 } },
        };

        public static readonly Dictionary<Model.Device, string> ConfigFileNames = new Dictionary<Model.Device, string>
        {
            { Model.Device.SPT961, "state_config961.txt"},
            { Model.Device.SPT962, "state_config961.txt"},
            { Model.Device.SPT963, "state_config963.txt"},
        };
    }
}

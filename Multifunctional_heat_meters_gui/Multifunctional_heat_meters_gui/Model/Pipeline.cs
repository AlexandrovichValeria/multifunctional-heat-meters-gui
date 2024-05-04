using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Pipeline : AbstractModel
    {
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public Dictionary<string, Parameter> Parameters
        {
            get { return _parameters; }
        }

        public Pipeline(bool active = false)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _parameters.Add("032н00", new Parameter("032н00", "042", "", ""));
            _parameters.Add("032н01", new Parameter("032н01", "16.31", "кгс/см2", "pressure")); //?
            _parameters.Add("032н02", new Parameter("032н02", "0", "", ""));
            _parameters.Add("032н08", new Parameter("032н08", "0", "кг/см2", "pressure"));
            _parameters.Add("033н00", new Parameter("033н00", "043", "", ""));
            _parameters.Add("033н01", new Parameter("033н01", "180", "'C", "temperature"));
            _parameters.Add("033н02", new Parameter("033н02", "0", "'C", "temperature"));
            _parameters.Add("034н00", new Parameter("034н00", "010", "", ""));
            _parameters.Add("034н01", new Parameter("034н01", "763.4", "м3/час", "time sensor dependant"));
            _parameters.Add("034н02", new Parameter("034н02", "0", "м3/час", "time sensor dependant"));
            _parameters.Add("034н06", new Parameter("034н06", "1000", "Гц", "")); //Только для "Частота вода"
            _parameters.Add("034н07", new Parameter("034н07", "0", "Гц", ""));    //Только для "Частота вода"
            _parameters.Add("034н08", new Parameter("034н08", "", "м3/имп", "impulse sensor dependant")); // Все, кроме "Частота вода"
            //Параметр 100 будет вноситься автоматически во время загрузки бд в файл(т.к. это номер трубопровода)
            _parameters.Add("101", new Parameter("101", "0", "", ""));
            _parameters.Add("102н00", new Parameter("102н00", "12", "", ""));
            _parameters.Add("102н03", new Parameter("102н03", "1", "", ""));
            _parameters.Add("104", new Parameter("104", "0", "", "pressure")); // Только для "Имп пар"
            _parameters.Add("105", new Parameter("105", "1", "", "")); // Только для "Имп пар"
            _parameters.Add("109н00", new Parameter("109н00", "0", "м3/час", "time sensor dependant"));
            _parameters.Add("109н01", new Parameter("109н01", "03401", "", ""));
            _parameters.Add("113н00", new Parameter("113н00", "4.5", "кгс/см2", "pressure"));
            _parameters.Add("113н01", new Parameter("113н01", "03202", "", ""));
            _parameters.Add("114н00", new Parameter("114н00", "70", "'C", "temperature"));
            _parameters.Add("114н01", new Parameter("114н01", "03302", "", ""));
            _parameters.Add("115н00", new Parameter("115н00", "11", "", ""));
            _parameters.Add("115н01", new Parameter("115н01", "0", "м3/ч", ""));
            _parameters.Add("120", new Parameter("120", "0", "т/час", ""));
            _parameters.Add("125н00", new Parameter("125н00", "", "'C", ""));
            _parameters.Add("125н01", new Parameter("125н01", "", "'C", ""));
            _parameters.Add("125н02", new Parameter("125н02", "", "кг/м3", ""));
            _parameters.Add("125н03", new Parameter("125н03", "", "кг/м3", ""));
            _parameters.Add("125н04", new Parameter("125н04", "", "кДж/кг", ""));
            _parameters.Add("125н05", new Parameter("125н05", "", "кДж/кг", ""));
            _parameters.Add("125н06", new Parameter("125н06", "", "мкПа*с", ""));
            _parameters.Add("125н07", new Parameter("125н07", "", "мкПа*с", ""));
        }
    }
}

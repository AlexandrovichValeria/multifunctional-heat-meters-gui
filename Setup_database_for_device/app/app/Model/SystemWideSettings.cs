using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup_database_for_device.Model
{
    public class SystemWideSettings
    {
        private int _pipelinesCount;

        private int _consumersCount;

        private Dictionary<string, Parameter> _parameters;

        public Dictionary<string, Parameter> Parameters
        {
            get { return _parameters; }
        }

        public SystemWideSettings(int pipelinesCount, int consumersCount)
        {
            _pipelinesCount = pipelinesCount;
            _consumersCount = consumersCount;
            _parameters = new Dictionary<string, Parameter>();
            _parameters.Add("003", new Parameter("003", "1050001295", "б/р"));
            _parameters.Add("004", new Parameter("004", "1050002299", "б/р"));
            _parameters.Add("008", new Parameter("008", "Нет данных?", ""));
            _parameters.Add("020", new Parameter("020", "Нет данных?", "дд-мм-гг"));
            _parameters.Add("021", new Parameter("021", "Нет данных?", "чч-мм-сс"));
            _parameters.Add("024", new Parameter("024", "23", "ч."));
            _parameters.Add("025", new Parameter("025", "24", "д."));
            _parameters.Add("030н00", new Parameter("030н00", "11", "кгс/см2, Гкал·ч, Гкал"));
            _parameters.Add("030н01", new Parameter("030н01", "0.01", "т"));
            _parameters.Add("030н02", new Parameter("030н02", "0.001", "Гкал"));
            _parameters.Add("031н00", new Parameter("031н00", new string('0', pipelinesCount), "б/р"));
            _parameters.Add("031н01", new Parameter("031н01", new string('0', consumersCount), "б/р"));
            _parameters.Add("035н00", new Parameter("035н00", "0", "'C"));
            _parameters.Add("035н01", new Parameter("035н01", "0", "б/р"));
            _parameters.Add("036н00", new Parameter("036н00", "4", "кгс/см2"));
            _parameters.Add("036н01", new Parameter("036н01", "0", "б/р"));
            _parameters.Add("037н00", new Parameter("037н00", "760", "мм.рт.ст"));
            _parameters.Add("037н01", new Parameter("037н01", "0", "б/р"));
            _parameters.Add("040н00", new Parameter("040н00", "20", "'C"));
            _parameters.Add("040н01", new Parameter("040н01", "0", "б/р"));
            _parameters.Add("038н00", new Parameter("038н00", "0", ""));
            _parameters.Add("038н01", new Parameter("038н01", "0", ""));
            _parameters.Add("038н02", new Parameter("038н02", "0", ""));
        }

        public int PipelinesCount
        {
            get { return _pipelinesCount; }
            set
            {
                _pipelinesCount = value;
                if (_parameters["031н00"].Value.Length < _pipelinesCount)
                {
                    string additional = new string('0', _pipelinesCount - _parameters["031н00"].Value.Length);
                    _parameters["031н00"].Value = _parameters["031н00"].Value + additional;
                }
            }
        }

        public int ConsumersCount
        {
            get { return _consumersCount; }
            set
            {
                _consumersCount = value;
                if (_parameters["031н01"].Value.Length < _consumersCount)
                {
                    string additional = new string('0', _consumersCount - _parameters["031н01"].Value.Length);
                    _parameters["031н01"].Value = _parameters["031н01"].Value + additional;
                }
            }
        }

        public void ChangeParameterValue(string parameterName, string value)
        {
            _parameters[parameterName].Value = value;
        }

        public void ChangeParameterUnitOfMeasurement(string parameterName, string unitOfMeasurement)
        {
            _parameters[parameterName].UnitOfMeasurement = unitOfMeasurement;
        }
    }
}

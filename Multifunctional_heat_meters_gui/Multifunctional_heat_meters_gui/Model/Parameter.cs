using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class Parameter
    {
        private string _name;

        private string _value;

        private string _unitOfMeasurement;

        private string _typeOfMeasurement;

        //Название параметра
        public string Name
        {
            get { return _name; }
        }

        //Значение параметра
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        //Единицы измерения
        public string UnitOfMeasurement
        {
            get { return _unitOfMeasurement; }
            set { _unitOfMeasurement = value; }
        }

        public string TypeOfMeasurement
        {
            get { return _typeOfMeasurement; }
            set { _typeOfMeasurement = value; }
        }

        public Parameter(string name, string value, string unitOfMeasurement, string typeOfMeasurement)
        {
            _name = name;
            _value = value;
            _unitOfMeasurement = unitOfMeasurement;
            _typeOfMeasurement = typeOfMeasurement;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup_database_for_device.Model
{
    public class Parameter
    {
        private string _name;

        private string _value;

        private string _unitOfMeasurement;

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

        public Parameter(string name, string value, string unitOfMeasurement)
        {
            _name = name;
            _value = value;
            _unitOfMeasurement = unitOfMeasurement;
        }
    }
}

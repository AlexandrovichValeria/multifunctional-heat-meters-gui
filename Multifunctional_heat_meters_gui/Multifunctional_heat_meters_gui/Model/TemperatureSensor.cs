﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
{
    public class TemperatureSensor : Sensor
    {
        /*private bool _active;

        private Dictionary<string, Parameter> _parameters;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        */
        /*public Dictionary<string, Parameter> Parameters
        {
            get { return _parameters; }
        }*/

        public TemperatureSensor(bool active = true)
        {
            _active = active;
            _parameters = new Dictionary<string, Parameter>();
            _parameters.Add("114н01", new Parameter("114н01", "03301", "", ""));
            _parameters.Add("033н00", new Parameter("033н00", "043", "", ""));
            _parameters.Add("033н01", new Parameter("033н01", "180", "'C", "temperature"));
            _parameters.Add("033н02", new Parameter("033н02", "0", "'C", "temperature"));
        }
    }
}

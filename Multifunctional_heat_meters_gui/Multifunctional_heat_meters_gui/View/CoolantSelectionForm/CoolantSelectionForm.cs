﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class CoolantSelectionForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private ComboBoxText combo2;
        [Builder.Object]
        private ComboBoxText combo3;
        [Builder.Object]
        private Box gas_box;
        [Builder.Object]
        private Box liquid_box;

        private GasBlock gas_block;
        private LiquidBlock liquid_block;

        private static readonly string SensorParamName = "034н00";

        public static CoolantSelectionForm Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.CoolantSelectionForm.CoolantSelectionForm.glade", null);
            return new CoolantSelectionForm(index, builder, builder.GetObject("form_box").Handle);
        }

        protected CoolantSelectionForm(int index, Builder builder, IntPtr handle) : base($"Теплоноситель {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _formIndex = index;
            button_box.Add(_backForwardComponent);

            gas_block = GasBlock.Create();
            gas_box.Add(gas_block);
            gas_box.Hide();

            liquid_block = LiquidBlock.Create();
            liquid_box.Add(liquid_block);
            liquid_box.Hide();

            SetupHandlers();
        }

        public Dictionary<string, string> GetCoolantSettings()
        {
            string flowMeter = "";
            if (combo2.ActiveId == "0")
                flowMeter = "0";
            else if (combo2.ActiveId == "1")
                flowMeter = "12";

            Dictionary<string, string> coolantSettings = new Dictionary<string, string>()
            {
                { "101", $"{combo1.ActiveId}" }, //тип теплоносителя
                { "102н00", flowMeter }, //тип расходомера
                { "034н00", $"{0}{Int32.Parse(combo3.ActiveId) + 1}{0}" }, //тип датчика
            };

            //if(combo1.ActiveId == "0")
            Dictionary<string, string> gasSettings = gas_block.GetGasSettings();
            Dictionary<string, string> liquidSettings = liquid_block.GetLiquidSettings();

            Dictionary<string, string> result = coolantSettings
                .Union(gasSettings)
                .Union(liquidSettings)
                .ToDictionary(x => x.Key, x => x.Value);

            return result;
        }

        protected override bool IsAbleToGoToNext()
        {
            if (!IsFormFilledOut())
                return false;
            string result = GetCoolantSettings()[SensorParamName];
            result = result.Substring(0, result.Length - 1);
            if (result != "")
            {
                paramsToNextForm = new Dictionary<string, string>()
                {
                    { "curIndicator", result }
                };
                return true;
            }
            return false;
        }

        public override bool IsFormFilledOut()
        {
            Dictionary<string, string> pars = GetCoolantSettings();
            if (pars["101"] == "" || pars["102н00"] == "" || pars["034н00"] == "000")
                return false;

            if (pars["101"] == "0")
                return true;

            if (pars["101"] == "1" || pars["101"] == "2")
            {
                if (pars["104"] == "" || pars["105"] == "")
                    return false;
                return true;
            }

            if (pars["101"] == "3")
            {
                if (pars["125н00"] == "" || pars["125н01"] == "" || pars["125н02"] == "" || pars["125н03"] == "" ||
                    pars["125н04"] == "" || pars["125н05"] == "" || pars["125н06"] == "" || pars["125н07"] == "")
                    return false;
                return true;
            }
            return false;
        }

        protected void SetupHandlers()
        {
            combo1.Changed += Combo1ChangedEvent;
            //DeleteEvent += OnLocalDeleteEvent;
        }
        protected void Combo1ChangedEvent(object sender, EventArgs a)
        {
            /*Console.WriteLine("CoolantData");
            Dictionary<string, string> dic = GetCoolantSettings();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                Console.WriteLine($"{kvp.Key}, {kvp.Value}");
            }*/

            switch (combo1.ActiveId)
            {
                case "0":
                    gas_box.Hide();
                    liquid_box.Hide();
                    break;
                case "1":
                    gas_box.Show();
                    liquid_box.Hide();
                    break;
                case "2":
                    gas_box.Show();
                    liquid_box.Hide();
                    break;
                case "3":
                    gas_box.Hide();
                    liquid_box.Show();
                    break;
            }
        }
    }
}

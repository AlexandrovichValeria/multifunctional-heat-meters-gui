﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View.SystemForm
{
    public class SystemForm : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box Checkboxes_box;
        [Builder.Object]
        private Box button_box;

        private ParticipatedPipelinesBlock _participatedPipelinesBlock;
        private ADS_97_Form _ADS_97_Form;
        //private BackForwardComponent BackForwardButtons;

        private int _minPipelinesCountFor_ADS_97 = 0;
        private static string SelectedPipelinesParam = "031н00";
        private Dictionary<string, string> ADS_97_result;

        public static SystemForm Create(Model.Device device)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.SystemForm.SystemForm.glade", null);
            return new SystemForm(device, builder, builder.GetObject("form_box").Handle);
        }

        protected SystemForm(Model.Device device, Builder builder, IntPtr handle) : base("Общесистемные параметры", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _ADS_97_Form = ADS_97_Form.Create();

            if (device == Model.Device.SPT963)
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(16, 8);
            else
                _participatedPipelinesBlock = ParticipatedPipelinesBlock.Create(12, 6);
            Checkboxes_box.Add(_participatedPipelinesBlock);


            //BackForwardButtons = BackForwardComponent.Create();
            button_box.Add(_backForwardComponent);

            SetupHandlers();
        }

        public string GetParamFromWindow(string param)
        {
            Dictionary<string, string> result = GetSystemWindowData();
            return result.ContainsKey(param) ? result[param] : null;
        }
        public Dictionary<string, string> GetSystemWindowData()
        {
            //Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> result = _participatedPipelinesBlock.GetResult();
            return result;
            /*Dictionary<string, string> result = _systemWindow.GetAllSystemSettings();
            foreach (KeyValuePair<string, string> param in ADS_97_result)
            {
                result.Add(param.Key, param.Value);
            }

            return result;*/
        }

        protected override void OnNextFormAction()
        {

            string zeroOneStringPipelines = GetParamFromWindow(SelectedPipelinesParam);
            int countSelectedPipelines = (zeroOneStringPipelines != null) ? GetPipelinesCountByOneZeroString(zeroOneStringPipelines) : 0;
            if (countSelectedPipelines > _minPipelinesCountFor_ADS_97)
            {
                MessageDialog dialog = new MessageDialog(_ADS_97_Form, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Dialog text");
                //_ADS_97_Form.ShowDialog();
            }
        }

        private int GetPipelinesCountByOneZeroString(string oneZeroString)
        {
            int count = 0;

            foreach (char sym in oneZeroString)
            {
                if (sym == '1')
                {
                    count++;
                }
            }

            return count;
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }
    }
}

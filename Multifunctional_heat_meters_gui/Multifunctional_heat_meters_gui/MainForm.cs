﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Gtk;

namespace Multifunctional_heat_meters_gui
{
    public class MainForm : Window
    {
        private Model.Device _device;
        private Model.Model _model;
        private bool _exitFlag;
        private LinkedList<View.WindowForm> _allForms = new LinkedList<View.WindowForm>();
        private ControllerManager controllerBuilder;
        private View.ADS_97_Form _ADS_97_Form;
        private int ADSAmount;
        private int _MaxChannel032Amount;
        private int _MaxChannel033Amount;
        private int _MaxChannel034Amount;

        private bool AutoValueCheck;

        private Builder _builder;

        #pragma warning disable 649

        [Builder.Object]
        private Paned paned1;
        [Builder.Object]
        private Box menu_box;
        [Builder.Object]
        private Box content_box;
        [Builder.Object]
        private MenuItem save;
        [Builder.Object]
        private MenuItem quit;
        [Builder.Object]
        private CheckMenuItem validation_switch;
        [Builder.Object]
        private ScrolledWindow scrolled;

#pragma warning restore 649

        public static MainForm Create(Model.Device device)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.MainForm.glade", null);
            return new MainForm(device, builder, builder.GetObject("window1").Handle);
        }
        protected MainForm(Model.Device device, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            //SetIconFromFile("./Resources/icon.ico");

            _device = device;
            _exitFlag = true;
            _model = new Model.Model(device);

            string deviceName = "";

            switch (_device)
            {
                case Model.Device.SPT961:
                    deviceName = "СПТ 961.1";
                    break;
                case Model.Device.SPT962:
                    deviceName = "СПТ 962";
                    break;
                case Model.Device.SPT963:
                    deviceName = "СПТ 963";
                    break;
                default:
                    break;
            }
            
            string window_name = "Настройщик базы данных " + deviceName;
            Title = window_name;

            string configFilePath = Dictionaries.ConfigFileNames[_device];
            string systemFormState = "";
            if (File.Exists(configFilePath))
            {
                systemFormState = File.ReadAllText(configFilePath);
            }

            View.SystemForm subForm1 = View.SystemForm.Create(1, device, systemFormState);
            View.SystemForm subForm2 = View.SystemForm.Create(2, device, systemFormState);
            CalculateMaxChannelsAmount(device);
            _ADS_97_Form = View.ADS_97_Form.Create();
            ADSAmount = 0;
            AutoValueCheck = false;

            _allForms.AddFirst(subForm1);
            _allForms.AddLast(subForm2);

            View.ContentMenu menu = View.ContentMenu.Create("Прибор " + deviceName);

            AppState appState = new AppState(_allForms, _ADS_97_Form);
            FormsBuilder formsBuilder = new FormsBuilder(_allForms);
            FormSwitcher formSwitcher = new FormSwitcher(menu, appState, content_box);
            MenuBuilder menuBuilder = new MenuBuilder(menu);
            controllerBuilder = new ControllerManager(appState, _model);

            formsBuilder.NewFormCreatedEvent += new EventHandler(formSwitcher.SetEventListenersForForm);
            formsBuilder.NewFormCreatedEvent += new EventHandler(OnNewFormCreated);

            formsBuilder.MenuShouldBeUpdatedEvent += new EventHandler<EventsArgs.MenuEventArgs>(menuBuilder.UpdateMenu);
            formsBuilder.MenuShouldBeUpdatedEvent += new EventHandler<EventsArgs.MenuEventArgs>(controllerBuilder.ResetControllers);
            subForm1.OccupiedChannelsChangedEvent += new EventHandler<List<int>>(CheckForADS);
            subForm2.OccupiedChannelsChangedEvent += new EventHandler<List<int>>(CheckForADS);
            subForm1.SystemFormChangedEvent += new EventHandler<Dictionary<string, string>>(subForm2.UpdateFromOtherForm);
            subForm2.SystemFormChangedEvent += new EventHandler<Dictionary<string, string>>(subForm1.UpdateFromOtherForm);

            _ADS_97_Form.ADSChanged += OnADSChanged;

            scrolled.Add(menu);

            SetupHandlers();
        }

        private void CalculateMaxChannelsAmount(Model.Device device)
        {
            List<int> channelAmounts = Dictionaries.MaxChannelAmountForDevice[device];
            _MaxChannel032Amount = channelAmounts[0];
            _MaxChannel033Amount = channelAmounts[1];
            _MaxChannel034Amount = channelAmounts[2];
        }

        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            save.Activated += OnSaveButtonActivated;
            quit.Activated += OnQuitButtonActivated;

            validation_switch.Toggled += OnParameterValidationButtonToggled;
            foreach (View.WindowForm form in _allForms)
            {
                form.SaveFormEvent += new EventHandler(OnSaveButtonActivated);
            }
        }

        protected void OnNewFormCreated(object form, EventArgs a)
        {
            View.WindowForm tempForm = (View.WindowForm)form;
            tempForm.SaveFormEvent += new EventHandler(OnSaveButtonActivated);
            tempForm.SetAutoValueCheck(AutoValueCheck);
        }

        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            View.WindowForm form = _allForms.First.Value;
            if (form is View.SystemForm)
            {
                Dictionary<string, string> data = ((View.SystemForm)form).GetSystemWindowData();
                string pipelines = data["031н00"];
                string consumers = data["031н01"];
                string measureSystem = data["030н00"];
                string configFilePath = Dictionaries.ConfigFileNames[_device];
                File.WriteAllText(configFilePath, pipelines + " " + consumers + " " + measureSystem);
            }

            Application.Quit();
            
            a.RetVal = true;
        }

        protected void OnADSChanged(object sender, EventArgs a)
        {
            ADSAmount = _ADS_97_Form.GetADSAmount();
        }

        protected void OnSaveButtonActivated(object sender, EventArgs a)
        {
            FileChooserNative fcn = new FileChooserNative("Сохранение базы данных", this, FileChooserAction.Save, "Сохранить", "Отмена");
            FileFilter filter = new FileFilter();
            filter.Name = "Configurator DB files";
            filter.AddPattern("*.xdb");
            fcn.AddFilter(filter);

            ResponseType response = (ResponseType)fcn.Run();

            if (response == ResponseType.Accept)
            {
                string selectedFilePath = fcn.Filename;

                controllerBuilder.saveDataFromAllForms();
                _model.SaveDataToFile(selectedFilePath, "");
            }

            fcn.Destroy();
        }

        private void OnQuitButtonActivated(object sender, EventArgs e)
        {
            DeleteEventArgs a = new DeleteEventArgs();
            OnLocalDeleteEvent(this, a);
            //Application.Quit();
        }

        private void CheckForADS(object sender, List<int> e)
        {
            int channels032 = e[0];
            int channels033 = e[1];

            /*if(channels032 > _MaxChannel032Amount + 8)
            {
                MessageDialog dialog = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "");
                dialog.Run();
                dialog.Destroy();
            }*/
            if (channels033 > _MaxChannel033Amount + 8)
            {
                MessageDialog dialog = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Недостаточно каналов 033");
                dialog.Run();
                dialog.Destroy();
            }
            else if (channels032 > _MaxChannel032Amount + ADSAmount * 4 || channels033 > _MaxChannel033Amount + ADSAmount * 4)
            {
                if(channels032 > _MaxChannel032Amount + 4 || channels033 > _MaxChannel033Amount + 4)
                {
                    _ADS_97_Form.ChangeAdaptersAmount(2);
                }
                _ADS_97_Form.Show();
            }
        }

        private void OnParameterValidationButtonToggled(object sender, EventArgs e)
        {
            if (validation_switch.Active)
            {
                validation_switch.Label = "_Отключить проверку значений";
                AutoValueCheck = true;
            }
            else
            {
                validation_switch.Label = "_Включить проверку значений";
                AutoValueCheck = false;
            }
            foreach (View.WindowForm form in _allForms)
            {
                form.SetAutoValueCheck(AutoValueCheck);
            }
        }
    }
}

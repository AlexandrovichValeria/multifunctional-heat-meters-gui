using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Gtk;

namespace Multifunctional_heat_meters_gui
{
    public class MainForm : Window
    {
        private Model.Device _device;
        private Model.Model _model;
        //private Controller.SystemController _sysController;
        private bool _exitFlag;
        private LinkedList<View.WindowForm> _allForms = new LinkedList<View.WindowForm>();
        private ControllerBuilder controllerBuilder;
        private View.ADS_97_Form _ADS_97_Form;
        //private int _minPipelinesCountFor_ADS_97 = 0;
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
        //[Builder.Object]
        //private Box main_box;
        [Builder.Object]
        private Box content_box;
        [Builder.Object]
        private MenuItem save;
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

            SetIconFromFile("./Resources/icon.ico");

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

            View.SystemForm subForm1 = View.SystemForm.Create(1, device);
            View.SystemForm subForm2 = View.SystemForm.Create(2, device);
            CalculateMaxChannelsAmount(device);
            _ADS_97_Form = View.ADS_97_Form.Create();
            ADSAmount = 0;
            AutoValueCheck = false;

            //_sysController = new Controller.SystemController(subForm1, _model);

            _allForms.AddFirst(subForm1);
            _allForms.AddLast(subForm2);

            View.ContentMenu menu = View.ContentMenu.Create("Прибор " + deviceName);

            AppState appState = new AppState(_allForms, _ADS_97_Form);
            FormsBuilder formsBuilder = new FormsBuilder(_allForms);
            FormSwitcher formSwitcher = new FormSwitcher(menu, appState, content_box);
            MenuBuilder menuBuilder = new MenuBuilder(menu);
            controllerBuilder = new ControllerBuilder(appState, _model);

            formsBuilder.NewFormCreatedEvent += new EventHandler(formSwitcher.SetEventListenersForForm);
            formsBuilder.NewFormCreatedEvent += new EventHandler(SetupHandlerForSaveButton);
            //formsBuilder.NewFormCreatedEvent += new EventHandler(controllerBuilder.SetNewControllerForForm);
            //formsBuilder.FormDeletedEvent += new EventHandler(controllerBuilder.DeleteControllerForm);

            formsBuilder.MenuShouldBeUpdatedEvent += new EventHandler<EventsArgs.MenuEventArgs>(menuBuilder.UpdateMenu);
            formsBuilder.MenuShouldBeUpdatedEvent += new EventHandler<EventsArgs.MenuEventArgs>(controllerBuilder.ResetControllers);
            subForm1.OccupiedChannelsChangedEvent += new EventHandler<List<int>>(CheckForADS);
            subForm2.OccupiedChannelsChangedEvent += new EventHandler<List<int>>(CheckForADS);
            subForm1.SystemFormChangedEvent += new EventHandler<Dictionary<string, string>>(subForm2.UpdateFromOtherForm);
            subForm2.SystemFormChangedEvent += new EventHandler<Dictionary<string, string>>(subForm1.UpdateFromOtherForm);

            _ADS_97_Form.ADSChanged += OnADSChanged;
            //subForm1.PowerSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(_sysController.ChangePowerSystem);
            //subForm1.PressureSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(_sysController.ChangePressureSystem);

            scrolled.Add(menu);
            //menu_box.ModifyBg(StateType.Normal, new Gdk.Color(253, 246, 227));
            //content_box.PackStart(subForm1, false, false, 0);
            
            SetupHandlers();
        }

        private void CalculateMaxChannelsAmount(Model.Device device)
        {
            List<int> channelAmounts = Dictionaries.MaxChannelAmountForDevice[device];
            _MaxChannel032Amount = channelAmounts[0];
            _MaxChannel033Amount = channelAmounts[1];
            _MaxChannel034Amount = channelAmounts[2];
        }

        /*private int GetAdsAmount()
        {
            return 0;
        }*/

        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            save.Activated += OnSaveButtonActivated;

            validation_switch.Toggled += OnParameterValidationButtonToggled;
            foreach (View.WindowForm form in _allForms)
            {
                form.SaveFormEvent += new EventHandler(OnSaveButtonActivated);
            }

            /*foreach (View.WindowForm form in _allForms)
            {
                form.SetValueCheckActiveEvent += new EventHandler(OnValueCheckActive);
                form.SetValueCheckInactiveEvent += new EventHandler(OnValueCheckInactive);
            }*/
            //back_button.Clicked += BackButtonClicked;
            //forward_button.Clicked += ForwardButtonClicked;
        }

        protected void SetupHandlerForSaveButton(object form, EventArgs a)
        {
            View.WindowForm tempForm = (View.WindowForm)form;

            tempForm.SaveFormEvent += new EventHandler(OnSaveButtonActivated);
            
        }

        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }

        protected void OnADSChanged(object sender, EventArgs a)
        {
            ADSAmount = _ADS_97_Form.GetADSAmount();
        }

        protected void OnSaveButtonActivated(object sender, EventArgs a)
        {
            MessageDialog dialog = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Имя файла и путь к нему не должны содержать кириллицу.");
            dialog.Run();
            dialog.Destroy();

            FileChooserDialog fileChooser = new FileChooserDialog(
            "Сохранение базы данных", // Dialog title
            null, // Parent window (can be null)
            FileChooserAction.Save, // Action type (Save)
            "Отмена", // Cancel button text
            ResponseType.Cancel, // Response type for the Cancel button
            "Сохранить", // Accept button text
            ResponseType.Accept // Response type for the Accept button
            );

            FileFilter filter = new FileFilter();
            filter.Name = "Configurator DB files";
            filter.AddPattern("*.xdb");
            fileChooser.AddFilter(filter);

            
            // Run the dialog and check the response
            ResponseType response = (ResponseType)fileChooser.Run();
            
            if (response == ResponseType.Accept)
            {
                // Get the selected file or file name
                string selectedFilePath = fileChooser.Filename;

                
                //GLib.FileUtils.GetFileContents.ut
                /*byte[] content = content.Select(Convert.ToByte).ToArray();
                string contentUtf8 = Encoding.UTF8.GetString(content);*/

                /*byte[] fnb = Encoding.GetEncoding("UTF-16").GetBytes(selectedFilePath);
                selectedFilePath = Encoding.GetEncoding("UTF-16").GetString(fnb);*/

                /*Encoding encoding = Encoding.GetEncoding("UTF-8"); // Replace with the desired encoding
                byte[] bytes = encoding.GetBytes(selectedFilePath);
                string convertedFilename = encoding.GetString(bytes);
                Console.WriteLine("selectedFilePath");
                Console.WriteLine(selectedFilePath);
                Console.WriteLine("convertedFilename");
                Console.WriteLine(convertedFilename);*/
                // Process the selected file or file name as needed
                // (e.g., save the file using StreamWriter)

                controllerBuilder.saveDataFromAllForms();
                _model.SaveDataToFile(selectedFilePath, "");
                // Clean up and destroy the dialog
                fileChooser.Destroy();
            }
            else
            {
                // Clean up and destroy the dialog
                fileChooser.Destroy();
                // Handle the case when the user cancels the dialog
            }
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

        /*private void OnValueCheckActive(object sender, EventArgs e)
        {
            AutoValueCheck = true;

            foreach (View.WindowForm form in _allForms)
            {
                form.SetAutoValueCheck(true);
            }
        }

        private void OnValueCheckInactive(object sender, EventArgs e)
        {
            AutoValueCheck = false;

            foreach (View.WindowForm form in _allForms)
            {
                form.SetAutoValueCheck(false);
            }
        }*/
    }
}

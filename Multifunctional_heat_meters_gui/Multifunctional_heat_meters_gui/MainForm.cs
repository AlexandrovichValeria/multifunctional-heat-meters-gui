using System;
using System.Collections.Generic;
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

        private Builder _builder;
        //View.SystemForm form = View.SystemForm.Create();
        //View.ContentMenu menu = View.ContentMenu.Create();

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

            View.SystemForm subForm1 = View.SystemForm.Create(device);
            View.SystemForm subForm2 = View.SystemForm.Create(device);
            
            //_sysController = new Controller.SystemController(subForm1, _model);

            _allForms.AddFirst(subForm1);

            View.ContentMenu menu = View.ContentMenu.Create("Прибор " + deviceName);


            AppState appState = new AppState(_allForms);
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

            //subForm1.PowerSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(_sysController.ChangePowerSystem);
            //subForm1.PressureSystemChangedEvent += new EventHandler<EventsArgs.MeasurementEventArgs>(_sysController.ChangePressureSystem);

            scrolled.Add(menu);
            //menu_box.ModifyBg(StateType.Normal, new Gdk.Color(253, 246, 227));
            //content_box.PackStart(subForm1, false, false, 0);
            
            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            save.Activated += OnSaveButtonActivated;
            foreach (View.WindowForm form in _allForms)
            {
                form.SaveFormEvent += new EventHandler(OnSaveButtonActivated);
            }
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

        protected void OnSaveButtonActivated(object sender, EventArgs a)
        {
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
    }
}

using System;
using System.Collections.Generic;
using Gtk;

namespace Multifunctional_heat_meters_gui
{
    public class MainForm : Window
    {
        private Model.Device _device;
        private Model.Model _model;
        private Controller.SystemController _sysController;
        private bool _exitFlag;
        private LinkedList<View.WindowForm> _allForms = new LinkedList<View.WindowForm>();

        /// <summary> Used to load in the glade file resource as a window. </summary>
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
        /*[Builder.Object]
        private Button back_button;
        [Builder.Object]
        private Button forward_button;*/

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

            View.SystemForm.SystemForm subForm1 = View.SystemForm.SystemForm.Create(device);
            View.SystemForm.SystemForm subForm2 = View.SystemForm.SystemForm.Create(device);
            
            _sysController = new Controller.SystemController(subForm1, _model);

            _allForms.AddFirst(subForm1);

            View.ContentMenu menu = View.ContentMenu.Create("Прибор " + deviceName);


            AppState appState = new AppState(_allForms);
            FormsBuilder formsBuilder = new FormsBuilder(_allForms);
            FormSwitcher formSwitcher = new FormSwitcher(menu, appState, content_box);
            MenuBuilder menuBuilder = new MenuBuilder(menu);
            formsBuilder.NewFormCreatedEvent += new EventHandler(formSwitcher.SetEventListenersForForm);
            formsBuilder.MenuShouldBeUpdatedEvent += new EventHandler<EventsArgs.MenuEventArgs>(menuBuilder.AddNewItemInMenu);

            //paned1.Add1(menu);
            menu_box.PackStart(menu, false, false, 0);
            //content_box.ModifyBg(StateType.Normal, new Gdk.Color(253, 246, 227));
            //content_box.PackStart(subForm1, false, false, 0);
            
            SetupHandlers();
        }

        /// <summary> Sets up the handlers. </summary>
        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            //back_button.Clicked += BackButtonClicked;
            //forward_button.Clicked += ForwardButtonClicked;
        }

        /// <summary> Handle Close of Form, Quit Application. </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="a">      Event information to send to registered event handlers. </param>
        protected void OnLocalDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }

        /// <summary> Handle Click of Button. </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="a">      Event information to send to registered event handlers. </param>
        protected void BackButtonClicked(object sender, EventArgs a)
        {
            
        }

        protected void ForwardButtonClicked(object sender, EventArgs a)
        {

        }

    }
}

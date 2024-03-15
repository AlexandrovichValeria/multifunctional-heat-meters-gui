using System;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class DeviceSelectionForm : Window
    {

        /// <summary> Used to load in the glade file resource as a window. </summary>
        private Builder _builder;

#pragma warning disable 649

        [Builder.Object]
        private Button button1;

        [Builder.Object]
        private ComboBoxText combo1;
        //private Entry StdInputTxt;
#pragma warning restore 649

        public static DeviceSelectionForm Create()
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.DeviceSelectionForm.glade", null);
            return new DeviceSelectionForm(builder, builder.GetObject("window1").Handle);
        }

        /// <summary> Specialised constructor for use only by derived class. </summary>
        /// <param name="builder"> The builder. </param>
        /// <param name="handle">  The handle. </param>
        protected DeviceSelectionForm(Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            SetupHandlers();
        }

        /// <summary> Sets up the handlers. </summary>
        protected void SetupHandlers()
        {
            DeleteEvent += OnLocalDeleteEvent;
            button1.Clicked += OnSendClick;
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
        protected void OnSendClick(object sender, EventArgs a)
        {
            Model.Device device = Model.Device.SPT961;

            switch (combo1.ActiveId)
            {
                case "0":
                    device = Model.Device.SPT961;
                    break;
                case "1":
                    device = Model.Device.SPT962;
                    break;
                case "2":
                    device = Model.Device.SPT963;
                    break;
            }
            MainForm mainWin = MainForm.Create(device);
            mainWin.Show();

            this.Hide();
        }

    }
}

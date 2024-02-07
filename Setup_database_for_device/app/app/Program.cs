using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gtk;

namespace Setup_database_for_device
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Gtk.Application.Init();
            DeviceSelectionForm win = DeviceSelectionForm.Create();
            //View.SystemForm win = View.SystemForm.Create();
            win.Show();
            Gtk.Application.Run();
        }
    }
}

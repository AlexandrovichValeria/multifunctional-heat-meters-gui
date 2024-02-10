using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Init();
            DeviceSelectionForm win = DeviceSelectionForm.Create();
            win.Show();
            Application.Run();
        }
    }
}
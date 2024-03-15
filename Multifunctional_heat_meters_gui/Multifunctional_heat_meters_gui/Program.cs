using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gtk;
using System.Text;

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
            //System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            Environment.SetEnvironmentVariable("G_FILENAME_ENCODING", "@locale");
            Environment.SetEnvironmentVariable("G_BROKEN_FILENAMES", "1");
            //Encoding = Encoding.UTF8;
            
            Application.Init();
            ApplyTheme();
            View.DeviceSelectionForm win = View.DeviceSelectionForm.Create();
            win.Show();
            Application.Run();
        }
        public static void ApplyTheme()
        {
            // Get the Global Settings
            var setts = Settings.Default;
            // This enables clear text on Win32, makes the text look a lot less crappy
            setts.XftRgba = "rgb";
            // This enlarges the size of the controls based on the dpi
            //setts.XftDpi = 96;
            // By Default Anti-aliasing is enabled, if you want to disable it for any reason set this value to 0
            //setts.XftAntialias = 0;
            // Enable text hinting
            setts.XftHinting = 1;
            //setts.XftHintstyle = "hintslight"
            setts.XftHintstyle = "hintfull";
            setts.FontName = "Verdana";
            

            // Load the Theme
            //Gtk.CssProvider css_provider = new Gtk.CssProvider();
            //css_provider.LoadFromPath("themes/DeLorean-3.14/gtk-3.0/gtk.css")
            //css_provider.LoadFromPath("themes/DeLorean-Dark-3.14/gtk-3.0/gtk.css");
            //Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css_provider, 800);
        }
    }
}

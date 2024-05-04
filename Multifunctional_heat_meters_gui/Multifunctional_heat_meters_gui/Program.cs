using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gtk;
using System.Text;
using System.IO;


namespace Multifunctional_heat_meters_gui
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Init();
            ApplyTheme();
            View.DeviceSelectionForm win = View.DeviceSelectionForm.Create();
            win.Show();
            Application.Run();
        }
        public static void ApplyTheme()
        {
            var setts = Settings.Default;
            setts.XftRgba = "rgb";
            setts.XftHinting = 1;
            //setts.XftHintstyle = "hintslight"
            setts.XftHintstyle = "hintfull";
            setts.FontName = "Verdana";

            
            string filePath = "Themes/MainTheme.css";
            string fileContent = File.ReadAllText(filePath);
            
            CssProvider css_provider = new CssProvider();
            css_provider.LoadFromData(fileContent);
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, css_provider, 800);
        }
    }
}

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui
{
    public class MenuBuilder
    {
        private View.ContentMenu _menu;

        public MenuBuilder(View.ContentMenu menu)
        {
            _menu = menu;
        }

        public void UpdateMenu(object sender, EventsArgs.MenuEventArgs args)
        {
            _menu.AddDeepButtonsByNumbers(args.ButtonName, args.ButtonsNumbers);
        }

    }
}

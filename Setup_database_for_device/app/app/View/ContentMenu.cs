using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Setup_database_for_device.View
{
    public class ContentMenu : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box box;

        private TreeStore ContentMenuStore;
        private TreeView tree;

        public event EventHandler FormChanged;

        private static readonly string s_topTreeGroupName = "ContentButtons";
        private static readonly string s_pipelineGroupName = "PipelinesButtons";
        private static readonly string s_consumersGroupName = "ConsumersButtons";

        private static readonly Dictionary<string, string> s_russianNames = new Dictionary<string, string>
        {
            { "PipelinesButtons", "т" },
            { "ConsumersButtons", "п" }
        };
        
        private enum TopButtonsTypes
        {
            SYSTEM,
            PIPELINES,
            CONSUMERS
        }

        private static readonly Dictionary<TopButtonsTypes, string> topButtonsNames = new Dictionary<TopButtonsTypes, string>()
        {
            { TopButtonsTypes.SYSTEM, "Общесистемные параметры" },
            { TopButtonsTypes.PIPELINES, "Настройка трубопроводов" },
            { TopButtonsTypes.CONSUMERS, "Настройка потребителей" },
        };

        public enum DeepButtonsNames
        {
            PIPELINES,
            CONSUMERS
        }

        private static readonly string[] s_pipelinesSettingsButtonsNames = new string[] { "Теплоноситель", "Первая настройка трубопровода", "Вторая настройка трубопровода" };

        public static ContentMenu Create(string deviceName)
        {
            Builder builder = new Builder(null, "Setup_database_for_device.View.ContentMenu.glade", null);
            return new ContentMenu(deviceName, builder, builder.GetObject("box").Handle);
        }

        protected ContentMenu(string deviceName, Builder builder, IntPtr handle) : base("Меню", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            tree = new TreeView();
            ContentMenuStore = new TreeStore(typeof(string));
            tree.Model = ContentMenuStore;
            Add(tree);

            TreeViewColumn topColumn = new TreeViewColumn();
            CellRendererText topCell = new CellRendererText();
            topColumn.PackStart(topCell, true);

            tree.AppendColumn(topColumn);

            topColumn.AddAttribute(topCell, "text", 0);

            //Gtk.TreeIter iter = ContentMenuStore.AppendValues("Общесистемные параметры");
            //ContentMenuStore.AppendValues(iter, "Fannypack");

            //iter = ContentMenuStore.AppendValues("Настройка трубопроводов");
            //ContentMenuStore.AppendValues(iter, "Nelly");

            ContentMenuStore.AppendValues(deviceName);
            ContentMenuStore.AppendValues("Общесистемные параметры");
            ContentMenuStore.AppendValues("Настройка трубопроводов");
            ContentMenuStore.AppendValues("Настройка потребителей");

            Gtk.TreePath path = new TreePath("1");
            tree.Selection.SelectPath(path);

            /*foreach (ContentMenuButton a in _topButtons)
            {
                ContentMenuStore.AppendValues(a);
            }*/

            SetupHandlers();
            ShowAll();
        }

        public void AddDeepButtonsInMenuByButtonsNumbers(DeepButtonsNames buttonName, List<int> buttonsNumbers)
        {
            Console.WriteLine("AddDeepButtonsInMenuByButtonsNumbers");
            string sectionName = "";
            string title = "";
            if (buttonName == DeepButtonsNames.CONSUMERS)
            {
                sectionName = "Настройка потребителей";
                title = "п";
            }
            else if (buttonName == DeepButtonsNames.PIPELINES)
            {
                sectionName = "Настройка трубопроводов";
                title = "т";
            }
            foreach(int number in buttonsNumbers)
            {
                //if (buttonName == DeepButtonsNames.PIPELINES)
                //{
                //AddPipelinesSettingsButtons(TreeViewItemsWithButtonInside[i], buttonsNumbers[i]);
                //}

                TreeIter parentIter;
                if (ContentMenuStore.GetIterFirst(out parentIter))
                {
                    do
                    {
                        string name = ContentMenuStore.GetValue(parentIter, 0).ToString();
                        if (name == sectionName)
                        {
                            Console.WriteLine("name == sectionName");

                            //Gtk.TreeIter childIter = ((TreeStore)ContentMenuStore).Append(ref parentIter);
                            //((Gtk.TreeStore)ContentMenuStore).SetValue(childIter, 0, "AAA");
                            
                            ContentMenuStore.AppendValues(parentIter, title + number.ToString());

                            break;
                        }
                    }
                    while (ContentMenuStore.IterNext(ref parentIter));
                }
            }
            tree.ExpandAll();

        }

        /*private List<ContentMenuButton> GetDeepButtonsByNumbers(DeepButtonsNames deepButtonName, List<int> deepButtonNumbers)
        {

            string buttonGroupName = "";
            string buttonTitle = "";

            switch (deepButtonName)
            {
                case DeepButtonsNames.PIPELINES:
                    buttonGroupName = s_pipelineGroupName;
                    buttonTitle = "т";
                    break;
                case DeepButtonsNames.CONSUMERS:
                    buttonGroupName = s_consumersGroupName;
                    buttonTitle = "п";
                    break;
                default:
                    break;
            }


            List<ContentMenuButton> buttons = new List<ContentMenuButton>(deepButtonNumbers.Count);

            foreach (int number in deepButtonNumbers)
            {
                ContentMenuButton currentButton = ContentMenuButton.Create($"{buttonTitle}{number}", buttonGroupName);
                //currentButton.SetWidth(140);
                currentButton.SetButtonType(deepButtonName);
                //currentButton.RadioButtonChecked += new EventHandler(ButtonClicked);
                buttons.Add(currentButton);
            }

            return buttons;

        }*/

        /*private void AddPipelinesSettingsButtons(TreeViewItem container, int pipelineNumber)
        {
            List<ContentMenuButton> pipelinesSettingsButtons = s_pipelinesSettingsButtonsNames.Select((string buttonName) =>
            {
                ContentMenuButton currentButton = ContentMenuButton.Create($"{buttonName} {pipelineNumber}", $"pipelinesSettings-{pipelineNumber}"); ;
                currentButton.SetWidth(120);
                //currentButton.EnableButton();
                //currentButton.RadioButtonChecked += new EventHandler(ButtonClicked);
                currentButton.DisableButton();

                //_pipelinesButtons.Add(currentButton);

                return currentButton;
            }).ToList();

            //pipelinesSettingsButtons[0].EnableButton();

            //foreach (TreeViewItem item in WrapButtonsInTreeViewItem(pipelinesSettingsButtons))
            //{
              //  container.Items.Add(item);
            //}
        }*/

        public void SelectButtonByName(string name)
        {
            TreeIter iter;
            bool found = ContentMenuStore.GetIterFirst(out iter);
            while (found)
            {
                string foundName = (string)ContentMenuStore.GetValue(iter, 0);

                if (foundName == name)
                {
                    tree.Selection.SelectIter(iter);
                    break;
                }
                found = ContentMenuStore.IterNext(ref iter);
            }
        }

        protected void SetupHandlers()
        {
            tree.Selection.Changed += new EventHandler(ButtonClicked);
            //DeleteEvent += OnLocalDeleteEvent;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("ButtonClicked");
            //tree.SelectionGet
            //ITreeModel model;
            //TreeIter iter;
            //Console.WriteLine(tree.Selection.GetSelected(out model, out iter));
            /*if(tree.Selection.GetSelected(out model, out iter))
            {
                string textValue = (string)model.GetValue(iter, 0);
                Console.WriteLine("Selected row text value: " + textValue);
            }*/
            FormChanged?.Invoke(sender, e);
        }
    }
}
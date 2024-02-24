using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    public class ContentMenu : TreeView
    {
        private Builder _builder;
        //[Builder.Object]
        //private Box box;
        //[Builder.Object]
        //private ScrolledWindow ScrolledWindow;

        private TreeStore ContentMenuStore;
        [Builder.Object]
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
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.ContentMenu.glade", null);
            return new ContentMenu(deviceName, builder, builder.GetObject("tree").Handle);
        }

        protected ContentMenu(string deviceName, Builder builder, IntPtr handle) : base()
        {
            _builder = builder;
            builder.Autoconnect(this);

            //tree = new TreeView();
            
            ContentMenuStore = new TreeStore(typeof(string));
            Model = ContentMenuStore;
            //Add(tree);
            //Label label = new Label("AAAAAAAAAAA");
            //Add(label);
            //tree.Vexpand = true;
            //tree.Hexpand = true;

            TreeViewColumn topColumn = new TreeViewColumn();
            CellRendererText topCell = new CellRendererText();
            topColumn.PackStart(topCell, true);

            AppendColumn(topColumn);

            topColumn.AddAttribute(topCell, "text", 0);

            //Gtk.TreeIter iter = ContentMenuStore.AppendValues("Общесистемные параметры");
            //ContentMenuStore.AppendValues(iter, "Fannypack");

            //iter = ContentMenuStore.AppendValues("Настройка трубопроводов");
            //ContentMenuStore.AppendValues(iter, "Nelly");

            ContentMenuStore.AppendValues(deviceName);
            ContentMenuStore.AppendValues("Общесистемные параметры");
            ContentMenuStore.AppendValues("Настройка трубопроводов");
            ContentMenuStore.AppendValues("Настройка потребителей");

            TreePath path = new TreePath("1");
            Selection.SelectPath(path);

            /*foreach (ContentMenuButton a in _topButtons)
            {
                ContentMenuStore.AppendValues(a);
            }*/

            SetupHandlers();
            ShowAll();
            //Console.WriteLine("label.IsVisible");
            //Console.WriteLine(label.IsVisible);
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
                TreeIter nodeIter;
                bool nodeExists = false;
                if (ContentMenuStore.GetIter(out nodeIter, new TreePath("2:0")))
                {
                    do
                    {
                        string node_name = ContentMenuStore.GetValue(nodeIter, 0).ToString();
                        if(node_name == title + number.ToString())
                        {
                            nodeExists = true;
                            break;
                        }
                    }
                    while (ContentMenuStore.IterNext(ref nodeIter));
                }

                if (!nodeExists)
                {
                    TreeIter parentIter;
                    if (ContentMenuStore.GetIterFirst(out parentIter))
                    {
                        do
                        {
                            string parent_name = ContentMenuStore.GetValue(parentIter, 0).ToString();
                            if (parent_name == sectionName)
                            {
                                ContentMenuStore.AppendValues(parentIter, title + number.ToString());
                                if (buttonName == DeepButtonsNames.PIPELINES)
                                {
                                    AddPipelinesSettingsButtons(number);
                                }

                                break;
                            }
                        }
                        while (ContentMenuStore.IterNext(ref parentIter));
                    }
                }
            }
            ExpandAll();

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

        private void AddPipelinesSettingsButtons(int pipelineNumber)
        {
            Console.WriteLine("pipelineNumber");
            Console.WriteLine(pipelineNumber);
            TreeIter nodeIter;
            string path = "2:0";
            if (ContentMenuStore.GetIter(out nodeIter, new TreePath(path)))
            {
                do
                {
                    string node_name = ContentMenuStore.GetValue(nodeIter, 0).ToString();
                    //Console.WriteLine("node_name");
                    //Console.WriteLine(node_name);
                    if (node_name == "т" + pipelineNumber)
                        foreach (string str in s_pipelinesSettingsButtonsNames)
                        {
                            ContentMenuStore.AppendValues(nodeIter, str + " " + pipelineNumber);
                        }
                }
                while (ContentMenuStore.IterNext(ref nodeIter));
            }
            //else 
              //  Console.WriteLine("Doesn't exist");
            
        }

        public void SelectButtonByName(string name)
        {
            TreeIter iter;
            bool found = ContentMenuStore.GetIterFirst(out iter);
            IterateTree(found, name, iter);
        }

        public void IterateTree(bool found, string name, TreeIter iter)
        {
            while (found)
            {
                string foundName = (string)ContentMenuStore.GetValue(iter, 0);

                if (foundName == name)
                {
                    Selection.SelectIter(iter);
                    break;
                }

                if (ContentMenuStore.IterHasChild(iter))
                {
                    TreeIter childIter;
                    bool iterfound = ContentMenuStore.IterChildren(out childIter, iter);
                    IterateTree(iterfound, name, childIter);
                }
                found = ContentMenuStore.IterNext(ref iter);
            }
        }

        protected void SetupHandlers()
        {
            Selection.Changed += new EventHandler(ButtonClicked);
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
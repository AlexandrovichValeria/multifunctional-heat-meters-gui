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

        private TreeStore ContentMenuStore;

        public event EventHandler FormChanged;
        public enum DeepButtonsNames
        {
            PIPELINES,
            SENSORS,
            CONSUMERS,
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
            
            ContentMenuStore = new TreeStore(typeof(string));
            Model = ContentMenuStore;

            TreeViewColumn topColumn = new TreeViewColumn();
            CellRendererText topCell = new CellRendererText();
            topColumn.PackStart(topCell, true);

            AppendColumn(topColumn);

            topColumn.AddAttribute(topCell, "text", 0);

            ContentMenuStore.AppendValues(deviceName);
            foreach(KeyValuePair<Dictionaries.TopButtonsTypes, string> keyValue in Dictionaries.topButtonNames)
                ContentMenuStore.AppendValues(keyValue.Value);

            TreePath path = new TreePath("1");
            Selection.SelectPath(path);

            SetupHandlers();
            ShowAll();
        }

        public void AddDeepButtonsByNumbers(DeepButtonsNames buttonName, List<int> buttonsNumbers)
        {
            string title = "";
            TreePath parentPath = new TreePath("");
            TreePath elementPath = new TreePath("");

            if (buttonName == DeepButtonsNames.CONSUMERS)
            {
                title = "п";
                parentPath = new TreePath("4");
                elementPath = new TreePath("4:0");
            }
            else if (buttonName == DeepButtonsNames.PIPELINES)
            {
                title = "т";
                parentPath = new TreePath("2");
                elementPath = new TreePath("2:0");
            }
            else if (buttonName == DeepButtonsNames.SENSORS)
            {
                title = "";
                parentPath = new TreePath("3");
                elementPath = new TreePath("3:0");
            }

            //delete current rows
            TreeIter nodeToDeleteIter;
            if (ContentMenuStore.GetIter(out nodeToDeleteIter, parentPath))
            {
                TreeIter childIter;
                bool iterFound = ContentMenuStore.IterChildren(out childIter, nodeToDeleteIter);

                while (iterFound)
                {
                    ContentMenuStore.Remove(ref childIter);
                    iterFound = ContentMenuStore.IterChildren(out childIter, nodeToDeleteIter);
                }
            }

            //add new rows
            foreach (int number in buttonsNumbers)
            {
                if(buttonName == DeepButtonsNames.SENSORS)
                    title = Dictionaries.sensorNames[number];
                TreeIter nodeIter;
                bool nodeExists = false;
                
                if (ContentMenuStore.GetIter(out nodeIter, elementPath))
                {
                    do
                    {
                        string node_name = ContentMenuStore.GetValue(nodeIter, 0).ToString();
                        if (node_name == title + number.ToString())
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
                    ContentMenuStore.GetIter(out parentIter, parentPath);
                    if (buttonName == DeepButtonsNames.SENSORS)
                        ContentMenuStore.AppendValues(parentIter, title);
                    else
                        ContentMenuStore.AppendValues(parentIter, title + number.ToString());

                    if (buttonName == DeepButtonsNames.PIPELINES)
                        AddPipelinesSettingsButtons(number);
                }
            }
            ExpandAll();
        }

        private void AddPipelinesSettingsButtons(int pipelineNumber)
        {
            TreeIter nodeIter;
            string path = "2:0";
            if (ContentMenuStore.GetIter(out nodeIter, new TreePath(path)))
            {
                do
                {
                    string node_name = ContentMenuStore.GetValue(nodeIter, 0).ToString();
                    if (node_name == "т" + pipelineNumber)
                        foreach (string str in s_pipelinesSettingsButtonsNames)
                        {
                            ContentMenuStore.AppendValues(nodeIter, str + " " + pipelineNumber);
                        }
                }
                while (ContentMenuStore.IterNext(ref nodeIter));
            }
            
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
            ITreeModel model;
            TreeIter iter;
            if(Selection.GetSelected(out model, out iter))
            {
                string name = (string)model.GetValue(iter, 0);
                SelectChild(name, iter);
            }
            FormChanged?.Invoke(sender, e);
        }

        public void SelectChild(string name, TreeIter iter)
        {
            if (ContentMenuStore.IterHasChild(iter) && name != Dictionaries.topButtonNames[Dictionaries.TopButtonsTypes.SENSORS])
            {
                TreeIter childIter;
                bool iterfound = ContentMenuStore.IterChildren(out childIter, iter);
                SelectChild(name, childIter);
            }
            else {
                Selection.SelectIter(iter); 
            }
        }
    }
}

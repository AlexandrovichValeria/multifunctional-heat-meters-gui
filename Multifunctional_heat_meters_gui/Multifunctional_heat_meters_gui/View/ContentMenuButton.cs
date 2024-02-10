using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class ContentMenuButton : Box
    {
        private Builder _builder;
        [Builder.Object]
        public RadioButton content_menu_button;
        //[Builder.Object]
        //private Expander expander;

        public event EventHandler RadioButtonChecked;
        private ContentMenu.DeepButtonsNames _buttonType;

        public static ContentMenuButton Create(string name, string groupName)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.ContentMenuButton.glade", null);
            return new ContentMenuButton(name, groupName, builder, builder.GetObject("expander").Handle);
        }

        protected ContentMenuButton(string name, string groupName, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            ButtonName = name;
            GroupName = groupName;
            content_menu_button.Label = name;
            //content_menu_button.Group = groupName;
            //content_menu_button.WidthRequest = 160;
            //ModifyBg(StateType.Normal, new Gdk.Color(20, 20, 20));

            SetupHandlers();
        }
        public void SetButtonType(ContentMenu.DeepButtonsNames typeName){
            _buttonType = typeName;
        }
        public ContentMenu.DeepButtonsNames TypeName => _buttonType;

        public string ButtonName { get; }
        public string GroupName { get; }
        public bool IsChecked => content_menu_button.Active;
        
        public void SetWidth(int width)
        {
            content_menu_button.WidthRequest = width;
        }

        public void DisableButton()
        {
            content_menu_button.Inconsistent = true;
        }

        public void CheckButton()
        {
            content_menu_button.Active = true;
            //SelectParentButton();
        }

        public void UncheckButton()
        {
            content_menu_button.Active = false;
            //UnselectChildrenButton();
        }

        public void SetGroup(ContentMenuButton main_button)
        {
            content_menu_button.JoinGroup(main_button.content_menu_button);
        }

        /*private void UnselectChildrenButton()
        {
            ContentMenuButton ContentButton = (ContentMenuButton)RadioButtonControl.Parent;
            
            Widget parent = ContentButton.Parent;

            if (parent.child != 0)
            {
                foreach (TreeViewItem item in TreeViewIt.Items)
                {
                    ContentMenuButton currentButton = (ContentMenuButton)item.Header;
                    if (currentButton.IsChecked)
                    {
                        currentButton.UncheckButton();
                    }
                }
            }
        }*/
        /*private void SelectParentButton()
        {
            ContentMenuButton ContentButton = (ContentMenuButton)RadioButtonControl.Parent;
            TreeViewItem TreeViewIt = (TreeViewItem)ContentButton.Parent;
            if (TreeViewIt.Parent.GetType() != typeof(TreeView))
            {
                TreeViewItem ParentTreeViewItem = (TreeViewItem)TreeViewIt.Parent;
                ContentMenuButton ParentContentButton = (ContentMenuButton)ParentTreeViewItem.Header;
                ParentContentButton.CheckButton();
            }
        }*/

        protected void SetupHandlers()
        {
            content_menu_button.Clicked += RadioButtonControl_Checked;
            //DeleteEvent += OnLocalDeleteEvent;
            //button1.Clicked += OnSendClick;
        }
        private void RadioButtonControl_Checked(object sender, EventArgs e)
        {
            /*ContentMenuButton ContentButton = (ContentMenuButton)RadioButtonControl.Parent;
            TreeViewItem TreeViewIt = (TreeViewItem)ContentButton.Parent;

            SelectParentButton();

            if (TreeViewIt.Items.Count != 0)
            {
                TreeViewIt.IsExpanded = true;
            }
            else
            {*/
                RadioButtonChecked?.Invoke(this, EventArgs.Empty);
            //}
        }

        private void RadioButtonControl_Unchecked(/*object sender, RoutedEventArgs e*/)
        {
            //UnselectChildrenButton();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class PipelineSettings2Form : WindowForm
    {
        private Builder _builder;
        [Builder.Object]
        private Box button_box;
        [Builder.Object]
        private CheckButton check1;
        [Builder.Object]
        private Entry entry1;
        [Builder.Object]
        private Entry entry2;
        [Builder.Object]
        private Label par115;
        [Builder.Object]
        private ComboBoxText combo1;
        [Builder.Object]
        private ComboBoxText combo2;

        public static PipelineSettings2Form Create(int index)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.PipelineSettings2Form.glade", null);
            return new PipelineSettings2Form(index, builder, builder.GetObject("form_box").Handle);
        }

        protected PipelineSettings2Form(int index, Builder builder, IntPtr handle) : base($"Вторая настройка трубопровода {index}", builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
            button_box.Add(_backForwardComponent);
            entry1.IsEditable = false;
            entry1.CanFocus = false;
            SetupHandlers();
        }

        public Dictionary<string, string> GetPipelineSettings2()
        {
            //string low_val = LowerValueTextBox.Text.Replace(',', '.');
            Dictionary<string, string> res = new Dictionary<string, string>()
            {
                { "115н00", $"{par115.Text}" },
                { "115н01", $"{entry1.Text}" },
                { "120", $"{entry2.Text}" },
            };
            return res;
        }

        protected void SetupHandlers()
        {
            check1.Clicked += OnCheck1Clicked;
            combo1.Changed += OnComboChanged;
            combo2.Changed += OnComboChanged;
            //DeleteEvent += OnLocalDeleteEvent;
        }

        private void OnComboChanged(object sender, EventArgs e)
        {
            if (sender == combo2 && combo2.ActiveId == "0")
            {
                MessageDialog dialog = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Вы выбрали вторую цифру '0', при отсутствии расхода в архивах будет нд.");
                dialog.Run();
                dialog.Destroy();

            }
            if ((combo1.ActiveId == "0" || combo1.ActiveId == "1") &&(combo2.ActiveId == "0" || combo2.ActiveId == "1"))
            {
                par115.Text = combo1.ActiveId + combo2.ActiveId;
            }
            else
            {
                par115.Text = "не определено";
            }
        }

        private void OnCheck1Clicked(object sender, EventArgs e)
        {
            if (check1.Active)
            {
                entry1.IsEditable = true;
                entry1.CanFocus = true;
            }
            else
            {
                entry1.IsEditable = false;
                entry1.CanFocus = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View
{
    class ParticipatedPipelinesBlock : WindowBlock
    {
        private Builder _builder;
        [Builder.Object]
        private Label ParticipatedPipelinesResult;
        [Builder.Object]
        private Label ParticipatedConsumersResult;
        [Builder.Object]
        private Box pipelines_box;
        [Builder.Object]
        private Box consumers_box;
        [Builder.Object]
        private ToggleButton pipelines_toggle;
        [Builder.Object]
        private ToggleButton consumers_toggle;

        private static string s_pipelinePrefix = "т";
        private static string s_consumerPrefix = "п";

        private string _pipelinesResult;
        private string _consumersResult;

        private CheckboxesBlock _participatedPipelinesCheckboxes;
        private CheckboxesBlock _participatedConsumersCheckboxes;

        public event EventHandler BlockChangedEvent;

        public static ParticipatedPipelinesBlock Create(int participatedPipelinesCount, int participatedConsumerCount, string CheckboxesState = "")
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.ParticipatedPipelinesBlock.glade", null);
            return new ParticipatedPipelinesBlock(participatedPipelinesCount, participatedConsumerCount, CheckboxesState, builder, builder.GetObject("box").Handle);
        }

        protected ParticipatedPipelinesBlock(int participatedPipelinesCount, int participatedConsumerCount, string CheckboxesState, Builder builder, IntPtr handle) : base(builder, handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            string[] elements = CheckboxesState.Split(' ');
            string pipelinesState = "";
            string consumersState = "";
            if(elements.Count() > 1)
            {
                pipelinesState = elements[0];
                consumersState = elements[1];
            }

            _participatedPipelinesCheckboxes = CheckboxesBlock.Create(participatedPipelinesCount, s_pipelinePrefix, pipelinesState);
            _participatedConsumersCheckboxes = CheckboxesBlock.Create(participatedConsumerCount, s_consumerPrefix, consumersState);

            if (elements[0].Length == participatedPipelinesCount && elements[1].Length == participatedConsumerCount)
            {
                ParticipatedPipelinesResult.Text = elements[0];
                ParticipatedConsumersResult.Text = elements[1];
                _pipelinesResult = elements[0];
                _consumersResult = elements[1];
            }
            else
            {
                ParticipatedPipelinesResult.Text = new string('0', participatedPipelinesCount);
                ParticipatedConsumersResult.Text = new string('0', participatedConsumerCount);
                _pipelinesResult = new string('0', participatedPipelinesCount);
                _consumersResult = new string('0', participatedConsumerCount);
            }

            pipelines_box.Add(_participatedPipelinesCheckboxes);
            consumers_box.Add(_participatedConsumersCheckboxes);

            ShowAll();
            
            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            _participatedPipelinesCheckboxes.CheckBoxesChecked += new EventHandler(ChangeParticipatedPipelinesResult);
            _participatedConsumersCheckboxes.CheckBoxesChecked += new EventHandler(ChangeParticipatedConsumersResult);
            _participatedPipelinesCheckboxes.CheckBoxesChecked += new EventHandler(OnBlockChanged);
            _participatedConsumersCheckboxes.CheckBoxesChecked += new EventHandler(OnBlockChanged);

            pipelines_toggle.Toggled += OnPipelinesToggleChanged;
            consumers_toggle.Toggled += onConsumersToggleChanged;
        }
        private void ChangeParticipatedPipelinesResult(object sender, EventArgs e)
        {
            _pipelinesResult = _participatedPipelinesCheckboxes.Result;
            ParticipatedPipelinesResult.Text = _pipelinesResult;
        }

        private void ChangeParticipatedConsumersResult(object sender, EventArgs e)
        {
            _consumersResult = _participatedConsumersCheckboxes.Result;
            ParticipatedConsumersResult.Text = _consumersResult;
        }

        private void OnBlockChanged(object sender, EventArgs e)
        {
            BlockChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void OnPipelinesToggleChanged(object sender, EventArgs e)
        {
            if (pipelines_toggle.Active)
            {
                pipelines_toggle.Label = "  Очистить  ";
                _participatedPipelinesCheckboxes.SelectAll();
            }
            else
            {
                pipelines_toggle.Label = "Выбрать все";
                _participatedPipelinesCheckboxes.DeselectAll();
            }
        }

        private void onConsumersToggleChanged(object sender, EventArgs e)
        {
            if (consumers_toggle.Active)
            {
                consumers_toggle.Label = "  Очистить  ";
                _participatedConsumersCheckboxes.SelectAll();
            }
            else
            {
                consumers_toggle.Label = "Выбрать все";
                _participatedConsumersCheckboxes.DeselectAll();
            }
        }

        public Dictionary<string, string> GetResult()
        {
            return new Dictionary<string, string>()
            {
                { "031н00", _pipelinesResult },
                { "031н01", _consumersResult }
            };
        }
        public override void SetData(Dictionary<string, string> data)
        {
            _participatedPipelinesCheckboxes.SetCheckboxes(data["031н00"]);
            _participatedConsumersCheckboxes.SetCheckboxes(data["031н01"]);
        }

        public bool SomeCheckboxesAreChecked()
        {
            string emptyPipelines = new string('0', _pipelinesResult.Length) ;
            string emptyConsumers = new string('0', _consumersResult.Length);
            if (_pipelinesResult == emptyPipelines || _consumersResult == emptyConsumers)
                return false;
            return true;
        }

    }
}

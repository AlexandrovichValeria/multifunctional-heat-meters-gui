using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace Multifunctional_heat_meters_gui.View.SystemForm
{
    class ParticipatedPipelinesBlock : Box
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

        private static string s_pipelinePrefix = "т";
        private static string s_consumerPrefix = "п";

        private string _pipelinesResult;
        private string _consumersResult;

        private CheckboxesBlock _participatedPipelinesCheckboxes;
        private CheckboxesBlock _participatedConsumersCheckboxes;


        public static ParticipatedPipelinesBlock Create(int participatedPipelinesCount, int participatedConsumerCount)
        {
            Builder builder = new Builder(null, "Multifunctional_heat_meters_gui.View.SystemForm.ParticipatedPipelinesBlock.glade", null);
            return new ParticipatedPipelinesBlock(participatedPipelinesCount, participatedConsumerCount, builder, builder.GetObject("box").Handle);
        }

        protected ParticipatedPipelinesBlock(int participatedPipelinesCount, int participatedConsumerCount, Builder builder, IntPtr handle) : base(handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            _participatedPipelinesCheckboxes = CheckboxesBlock.Create(participatedPipelinesCount, s_pipelinePrefix);
            _participatedConsumersCheckboxes = CheckboxesBlock.Create(participatedConsumerCount, s_consumerPrefix);

            ParticipatedPipelinesResult.Text = new string('0', participatedPipelinesCount);
            ParticipatedConsumersResult.Text = new string('0', participatedConsumerCount);

            _pipelinesResult = new string('0', participatedPipelinesCount);
            _consumersResult = new string('0', participatedConsumerCount);

            pipelines_box.Add(_participatedPipelinesCheckboxes);
            consumers_box.Add(_participatedConsumersCheckboxes);

            
            SetupHandlers();
        }

        protected void SetupHandlers()
        {
            //DeleteEvent += OnLocalDeleteEvent;
            _participatedPipelinesCheckboxes.CheckBoxesChecked += new EventHandler(ChangeParticipatedPipelinesResult);
            _participatedConsumersCheckboxes.CheckBoxesChecked += new EventHandler(ChangeParticipatedConsumersResult);
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

        public Dictionary<string, string> GetResult()
        {
            return new Dictionary<string, string>()
            {
                { "031н00", _pipelinesResult },
                { "031н01", _consumersResult }
            };
        }

    }
}

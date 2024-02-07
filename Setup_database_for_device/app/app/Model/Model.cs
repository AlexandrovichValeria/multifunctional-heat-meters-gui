using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup_database_for_device.Model
{
    public enum Device
    {
        SPT961,
        SPT962,
        SPT963
    }

    public class Model
    {
        private SystemWideSettings _systemWideSettings;

        private List<Pipeline> _pipelines;

        private List<Consumer> _consumers;

        private Device _device;
        public SystemWideSettings SystemWideSettings
        {
            get { return _systemWideSettings; }
        }

        public Device CurrentDevice
        {
            get { return _device; }
            set
            {
                _device = value;
                int newPipelinesCount = 12;
                int newConsumersCount = 6;
                if (_device == Device.SPT963)
                {
                    newPipelinesCount = 16;
                    newConsumersCount = 8;
                }
                _systemWideSettings.PipelinesCount = newPipelinesCount;
                _systemWideSettings.ConsumersCount = newConsumersCount;
                foreach (var consumer in _consumers)
                {
                    consumer.PipelinesCount = newPipelinesCount;
                }
                if (_pipelines.Count < newPipelinesCount)
                {
                    for (int i = 0; i < newPipelinesCount - _pipelines.Count; i++)
                    {
                        _pipelines.Add(new Pipeline());
                        string number = (i + 1).ToString();
                        if (number.Length == 1) number = "0" + number;
                        _pipelines[i].ChangeParameterValue("109н01", "034" + number);
                        _pipelines[i].ChangeParameterValue("113н01", "032" + number);
                        _pipelines[i].ChangeParameterValue("114н01", "033" + number);
                    }
                }
                if (_consumers.Count < newConsumersCount)
                {
                    for (int i = 0; i < newConsumersCount - _consumers.Count; i++)
                    {
                        _consumers.Add(new Consumer(newPipelinesCount));
                    }
                }
            }
        }

        public Model(Device device)
        {
            _device = device;
            int pipelinesCount = 12;
            int consumersCount = 6;
            if (_device == Device.SPT963)
            {
                pipelinesCount = 16;
                consumersCount = 8;
            }
            _pipelines = new List<Pipeline>();
            for (int i = 0; i < pipelinesCount; i++)
            {
                _pipelines.Add(new Pipeline());
                string number = (i + 1).ToString();
                if (number.Length == 1) number = "0" + number;
                _pipelines[i].ChangeParameterValue("109н01", "034" + number);
                _pipelines[i].ChangeParameterValue("113н01", "032" + number);
                _pipelines[i].ChangeParameterValue("114н01", "033" + number);
            }
            _consumers = new List<Consumer>();
            for (int i = 0; i < consumersCount; i++)
            {
                _consumers.Add(new Consumer(pipelinesCount));
            }
            _systemWideSettings = new SystemWideSettings(pipelinesCount, consumersCount);
            
        }

        public Consumer GetConsumerByInd(int index)
        {
            return _consumers[index];
        }

        public Pipeline GetPipelineByInd(int index)
        {
            return _pipelines[index];
        }

    }
}

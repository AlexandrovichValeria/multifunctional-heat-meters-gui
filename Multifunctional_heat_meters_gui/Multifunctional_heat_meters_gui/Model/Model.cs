using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multifunctional_heat_meters_gui.Model
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

        private Dictionary<string, Sensor> _sensors;

        private List<Consumer> _consumers;

        private Device _device;

        private List<int> ParticipatedChannels;
        private List<int> Participated032Channels;
        private List<int> Participated033Channels;

        private int _channelsCount;

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
                int newChannelCount = 16;
                if (_device == Device.SPT963)
                {
                    newPipelinesCount = 16;
                    newConsumersCount = 8;
                }
                _systemWideSettings.PipelinesCount = newPipelinesCount;
                _systemWideSettings.ConsumersCount = newConsumersCount;
                _systemWideSettings.ChannelsCount = newChannelCount;
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
            int channelsCount = 16;
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

            _channelsCount = channelsCount;

            ParticipatedChannels = new List<int>();
            Participated032Channels = new List<int>();
            Participated033Channels = new List<int>();

            for (int i = 0; i < _channelsCount; i++)
            {
                ParticipatedChannels.Add(0);
                Participated032Channels.Add(0);
                Participated033Channels.Add(0);
            }

            //Создание датчиков
            _sensors = new Dictionary<string, Sensor>();
            
            for (int i = 1; i <= 4; i++)
            {
                //if (i == 3)
                //    continue;
                Sensor temp_sensor = new Sensor(i);
                _sensors.Add(Dictionaries.sensorNames[i], temp_sensor);

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

        public Sensor GetSensorByName(string name)
        {
            return _sensors[name];
        }

        public void SaveDataToFile(string path, string serialNumber)
        {
            ParticipatedChannels = new List<int>();
            Participated032Channels = new List<int>();
            Participated033Channels = new List<int>();

            for (int i = 0; i < _channelsCount; i++)
            {
                ParticipatedChannels.Add(0);
                Participated032Channels.Add(0);
                Participated033Channels.Add(0);
            }

            string targetDevice = "";
            switch (_device)
            {
                case Device.SPT961:
                    targetDevice = "TSPT961_1";
                    break;
                case Device.SPT962:
                    targetDevice = "TSPT962";
                    break;
                case Device.SPT963:
                    targetDevice = "TSPT963";
                    break;
                default:
                    break;
            }

            DB.DateBase dataBase = new DB.DateBase(serialNumber, targetDevice, "0");
            for (int i = 0; i < _pipelines.Count; i++)
            {
                if (_pipelines[i].Active == true)
                {
                    ParticipatedChannels[i] = 1;
                    Participated032Channels[i] = 1;
                    Participated033Channels[i] = 1;
                }
                else
                {
                    //_pipelines[i].ChangeParameterValue("109н01", "0");
                }
            }

            //Внесение общесистемных параметров
            
            DB.Channel systemWideChannel = new DB.Channel("0", "0", "Common", "0", "системный канал");
            Dictionary<string, Parameter> parameters = _systemWideSettings.Parameters;

            foreach (KeyValuePair<string, Sensor> name_sensor in _sensors)
            {
                Sensor currentSensor = name_sensor.Value;
                string param_name = "";
                string channel_name1 = "";
                switch (currentSensor.Type)
                {
                    case 1:
                        param_name = "035н01";
                        channel_name1 = "033";
                        break;
                    case 2:
                        param_name = "036н01";
                        channel_name1 = "032";
                        break;
                    case 3:
                        param_name = "037н01";
                        channel_name1 = "032";
                        break;
                    case 4:
                        param_name = "040н01";
                        channel_name1 = "033";
                        break;
                }
                if (currentSensor.Active == false)
                {
                    parameters[param_name] = new Parameter(param_name, "0", "", "");
                    //parameters.Add(param_name, new Parameter(param_name, "0", "", ""));
                    continue;
                }
                else
                {
                    //int channelnumber = OccupyChannel();
                    int channelnumber = OccupyChannel(channel_name1);
                    currentSensor.ChannelNumber = channelnumber;
                    string channel_name2 = channelnumber.ToString();
                    if (channel_name2.Length == 1)
                        channel_name2 = "0" + channel_name2;
                    parameters[param_name] = new Parameter(param_name, $"{channel_name1}{channel_name2}", "", "");
                    //parameters.Add(param_name, new Parameter(param_name, $"{channel_name1}{channel_name2}", "", ""));
                }
            }

            List<DB.TagGroup> tagGroups = new List<DB.TagGroup>();
            foreach (var item in parameters)
            {
                string name = item.Key;
                Parameter parameter = item.Value;
                if (name.Contains("н") == false)
                {
                    systemWideChannel.AddTag(new DB.Tag(Int32.Parse(name), name, parameter.Value, "", parameter.UnitOfMeasurement));
                }
                else
                {
                    int ordinal = Int32.Parse(name.Substring(0, 3));
                    int index = Int32.Parse(name.Substring(4, 2));
                    DB.TagGroup currentTagGroup = null;
                    foreach (var tagGroup in tagGroups)
                    {
                        if (tagGroup.Ordinal == ordinal) currentTagGroup = tagGroup;
                    }
                    if (currentTagGroup == null)
                    {
                        currentTagGroup = new DB.TagGroup(ordinal);
                        tagGroups.Add(currentTagGroup);
                    }
                    currentTagGroup.AddNewTag(new DB.GroupTag(index, name, parameter.Value, "", parameter.UnitOfMeasurement));
                }
            }
            
            foreach (var tagGroup in tagGroups)
            {
                systemWideChannel.AddTagGroup(tagGroup);
            }
            dataBase.AddChannel(systemWideChannel);

            //Внесение информации по трубопроводам
            List<DB.Channel> channelsListT = new List<DB.Channel>();
            List<DB.Channel> channelsListK = new List<DB.Channel>();
            for (int i = 0; i < _pipelines.Count; i++)
            {
                if (_pipelines[i].Active == false) continue;

                //ParticipatedChannels[i] = 1;
                Pipeline currentPipeline = _pipelines[i];
                parameters = currentPipeline.Parameters;
                bool freqWater = false; // относится ли к листу "Чатота вода"
                bool impSteam = false; //относится ли к листу "Имп пар"
                bool param125needed = false; // нужно ли вводить параметры 125н*;
                if (parameters["034н00"].Value.Contains("3") || parameters["034н00"].Value.Contains("4")) // Условие перехода на лист "Частота вода"
                    freqWater = true;
                if (parameters["101"].Value.Contains("1") || parameters["101"].Value.Contains("2")) // Условие перехода на лист "Частота вода"
                    impSteam = true;
                if (parameters["101"].Value.Contains("3"))
                    param125needed = true;
                tagGroups = new List<DB.TagGroup>();
                string suffixT = "т" + (i + 1).ToString();
                string suffixK = "к" + (i + 1).ToString();
                DB.Channel channelT = new DB.Channel((i + 1).ToString(), suffixT, "Channel", "т", "трубопровод");
                DB.Channel channelK = new DB.Channel((i + 1).ToString(), suffixK, "Channel", "к", "доп.канал");
                channelT.AddTag(new DB.Tag(100, "100" + suffixT, (i + 1).ToString(), "", "")); // номер трубопровода
                foreach (var item in parameters)
                {
                    string name = item.Key;
                    Parameter parameter = item.Value;

                    if (name.StartsWith("125") && param125needed == false) // Если параметр 125 не нужен
                        continue;

                    if (name == "034н06" || name == "034н07") // Только для листа "Частота вода"
                        if (freqWater == false)
                            continue;

                    if (name == "104" || name == "105") // Только для листа "Имп Пар"
                        if (impSteam == false)
                            continue;

                    if (name == "034н08" && freqWater == true) // Для всех, кроме "Частота вода"
                        continue;

                    if (name.Contains("н") == false)
                    {
                        channelT.AddTag(new DB.Tag(Int32.Parse(name), name + suffixT, parameter.Value, "", parameter.UnitOfMeasurement));
                    }
                    else
                    {
                        int ordinal = Int32.Parse(name.Substring(0, 3));
                        int index = Int32.Parse(name.Substring(4, 2));
                        DB.TagGroup currentTagGroup = null;
                        foreach (var tagGroup in tagGroups)
                        {
                            if (tagGroup.Ordinal == ordinal) currentTagGroup = tagGroup;
                        }
                        if (currentTagGroup == null)
                        {
                            currentTagGroup = new DB.TagGroup(ordinal);
                            tagGroups.Add(currentTagGroup);
                        }
                        if (ordinal >= 30 && ordinal < 100) // параметры для каналов
                        {
                            currentTagGroup.AddNewTag(new DB.GroupTag(index, name + suffixK, parameter.Value, "", parameter.UnitOfMeasurement));
                        }
                        else //параметры трубопроводов
                        {
                            currentTagGroup.AddNewTag(new DB.GroupTag(index, name + suffixT, parameter.Value, "", parameter.UnitOfMeasurement));
                        }
                    }
                }
                foreach (var tagGroup in tagGroups)
                {
                    if (tagGroup.Ordinal >= 30 && tagGroup.Ordinal < 100)
                        channelK.AddTagGroup(tagGroup);
                    else
                        channelT.AddTagGroup(tagGroup);
                }
                channelsListK.Add(channelK);
                channelsListT.Add(channelT);
            }

            foreach (var channel in channelsListT)
            {
                dataBase.AddChannel(channel);
            }
            
            foreach (var channel in channelsListK)
            {
                dataBase.AddChannel(channel);
            }

            //Внесение информации по датчикам
            List<DB.Channel> channelsListS = new List<DB.Channel>();

            foreach(KeyValuePair<string, Sensor> name_sensor in _sensors)
            {
                Sensor currentSensor = name_sensor.Value;
                if (currentSensor.Active == false)
                {
                    continue; 
                }
                parameters = currentSensor.Parameters;
                tagGroups = new List<DB.TagGroup>();
                //string suffixT = "т" + channelnumber.ToString();
                string suffixK = "к" + currentSensor.ChannelNumber.ToString();
                //DB.Channel channelTS = new DB.Channel(channelnumber.ToString(), suffixT, "Channel", "т", "трубопровод");
                DB.Channel channelS = new DB.Channel(currentSensor.ChannelNumber.ToString(), suffixK, "Channel", "к", "доп.канал");

                foreach (var item in parameters)
                {
                    string name = item.Key;
                    Parameter parameter = item.Value;

                    int ordinal = Int32.Parse(name.Substring(0, 3));
                    int index = Int32.Parse(name.Substring(4, 2));
                    DB.TagGroup currentTagGroup = null;
                    foreach (var tagGroup in tagGroups)
                    {
                        if (tagGroup.Ordinal == ordinal) currentTagGroup = tagGroup;
                    }
                    if (currentTagGroup == null)
                    {
                        currentTagGroup = new DB.TagGroup(ordinal);
                        tagGroups.Add(currentTagGroup);
                    }
                    if (ordinal >= 30 && ordinal < 100) // параметры для каналов
                        currentTagGroup.AddNewTag(new DB.GroupTag(index, name + suffixK, parameter.Value, "", parameter.UnitOfMeasurement));
                    //else //параметры трубопроводов
                      //  currentTagGroup.AddNewTag(new DB.GroupTag(index, name + suffixT, parameter.Value, "", parameter.UnitOfMeasurement));
                }
                foreach (var tagGroup in tagGroups)
                {
                    if (tagGroup.Ordinal >= 30 && tagGroup.Ordinal < 100)
                        channelS.AddTagGroup(tagGroup);
                }
                channelsListS.Add(channelS);

            }

            foreach (var channel in channelsListS)
            {
                dataBase.AddChannel(channel);
            }

            //Внесение информации по потребителям
            for (int i = 0; i < _consumers.Count; i++)
            {
                if (_consumers[i].Active == false) continue;
                string suffixP = "п" + (i + 1).ToString();
                DB.Channel consumerChannel = new DB.Channel((i + 1).ToString(), suffixP, "Group", "п", "магистраль");
                Consumer currentConsumer = _consumers[i];
                consumerChannel.AddTag(new DB.Tag(300, "300" + suffixP, currentConsumer.Id.ToString(), "", ""));
                string param301 = "";
                for (int j = 0; j < _systemWideSettings.PipelinesCount; j++)
                {
                    if (_pipelines[j].Active == false)
                    {
                        param301 = param301 + "0";
                    }
                    else
                    {
                        param301 = param301 + ((int)currentConsumer.GetPipelineStatusByInd(j)).ToString();
                    }
                }
                param301 = param301 + currentConsumer.AccountingSchemeNumber.ToString();
                consumerChannel.AddTag(new DB.Tag(301, "301" + suffixP, param301, "", ""));
                dataBase.AddChannel(consumerChannel);
            }

            dataBase.SaveDBToFile(path, "xdb");
        }

        private int OccupyChannel()
        {
            int result = 0;
            
            for(int i = 0; i < ParticipatedChannels.Count; i++)
            {
                if (ParticipatedChannels[i] == 1)
                    continue;
                else
                {
                    result = i + 1;
                    ParticipatedChannels[i] = 1;
                    break;
                }
            }
            
            return result;
        }

        private int OccupyChannel(string channel_name)
        {
            int result = 0;

            if(channel_name == "032")
            {
                for (int i = 0; i < Participated032Channels.Count; i++)
                {
                    if (Participated032Channels[i] == 1)
                        continue;
                    else
                    {
                        result = i + 1;
                        Participated032Channels[i] = 1;
                        break;
                    }
                }
            }
            else if(channel_name == "033")
            {
                for (int i = 0; i < Participated033Channels.Count; i++)
                {
                    if (Participated033Channels[i] == 1)
                        continue;
                    else
                    {
                        result = i + 1;
                        Participated033Channels[i] = 1;
                        break;
                    }
                }
            }

            return result;
        }
    }
}

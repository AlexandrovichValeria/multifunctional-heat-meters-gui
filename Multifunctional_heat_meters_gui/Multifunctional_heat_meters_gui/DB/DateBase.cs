using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Multifunctional_heat_meters_gui.DB
{
    class DateBase
    {

        private static int s_maxChannelsInDateBase = 100;

        private string _serialNumber;
        private string _targetDevice;
        private string _id;
        private XElement _dateBaseXML;
        private List<Channel> _channelList;

        public DateBase(string serialNumber, string targetDevice, string id)
        {
            _serialNumber = serialNumber;
            _targetDevice = targetDevice;
            _id = id;
            _channelList = new List<Channel>(s_maxChannelsInDateBase);
            _dateBaseXML = new XElement("TagList");
            _dateBaseXML.Add(new XAttribute("SerialNumber", _serialNumber), new XAttribute("TargetDevice", _targetDevice), new XAttribute("Id", _id));

        }

        public string SerialNumber
        {
            get { return _serialNumber; }
        }

        public string TargetDevice
        {
            get { return _targetDevice; }
        }

        public string Id
        {
            get { return _id; }
        }

        public void AddChannel(Channel channel)
        {
            _channelList.Add(channel);
        }

        public Channel[] GetChannelListByPrefix(string prefix)
        {


            Channel[] result = _channelList.Select((Channel channel) =>
            {
                return channel.Prefix == prefix ? channel : null;
            }).ToArray();

            return result;

        }

        public Channel getChannelByName(string name)
        {
            foreach (Channel channel in _channelList)
            {
                if (channel.Name == name) return channel;
            }

            return null;
        }

        public AbstractTag GetTagById(string id)
        {
            foreach (Channel channel in _channelList)
            {
                AbstractTag tag = channel.GetTagById(id);
                if (!(tag is null)) return tag;
            }

            return null;
        }

        public void SaveDBToFile(string fileName, string extenstion)
        {
            XDocument xdoc = new XDocument();

            _dateBaseXML.RemoveNodes();
            foreach (Channel channel in _channelList) _dateBaseXML.Add(channel.XML);
            xdoc.Add(_dateBaseXML);
            xdoc.Save($"{fileName}.{extenstion}");
        }
    }
}

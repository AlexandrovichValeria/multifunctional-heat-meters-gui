using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Multifunctional_heat_meters_gui.DB
{
    class TagGroup
    {

        private static int s_maxCountElementsInTagGroup = 150;

        private int _ordinal;
        private string _name;
        private XElement _XML;
        private List<GroupTag> _tagList;

        public TagGroup(int ordinal, string name = "")
        {
            _ordinal = ordinal;
            _name = name;
            _XML = new XElement("TagGroup");
            _tagList = new List<GroupTag>(s_maxCountElementsInTagGroup);

            _XML.Add(new XAttribute("Ordinal", _ordinal), new XAttribute("Name", _name));

        }

        public int Ordinal
        {
            get { return _ordinal; }
        }

        public string Name
        {
            get { return _name; }
        }

        public XElement XML
        {
            get
            {
                _XML.RemoveNodes();
                foreach (GroupTag tag in _tagList) _XML.Add(tag.XML);
                return _XML;
            }
        }

        public void AddNewTag(GroupTag tag)
        {
            _tagList.Add(tag);
        }

        public GroupTag GetGroupTagByIndex(int index)
        {
            foreach (GroupTag tag in _tagList)
            {
                if (tag.Index == index) return tag;
            }

            return null;
        }

        public GroupTag GetGroupTagById(string id)
        {
            foreach (GroupTag tag in _tagList)
            {
                if (tag.Id == id) return tag;
            }

            return null;
        }

    }
}

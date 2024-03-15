using System.Collections.Generic;
using System.Xml.Linq;

namespace Multifunctional_heat_meters_gui.DB
{
    class Channel
    {
        private static int s_maxTagCountInChannel = 100;

        private string _no;
        private string _name;
        private string _kind;
        private string _prefix;
        private string _description;
        private List<Tag> _tagList;
        public List<TagGroup> _tagGroupList;
        private XElement _XML;

        public Channel(string no, string name, string kind, string prefix, string description)
        {
            _no = no;
            _name = name;
            _kind = kind;
            _prefix = prefix;
            _description = description;
            _tagList = new List<Tag>(s_maxTagCountInChannel);
            _tagGroupList = new List<TagGroup>(s_maxTagCountInChannel);
            _XML = createTag();
        }

        public string No
        {
            get { return _no; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Kind
        {
            get { return _kind; }
        }

        public string Prefix
        {
            get { return _prefix; }
        }

        public string Description
        {
            get { return _description; }
        }

        public XElement XML
        {
            get
            {
                _XML.RemoveNodes();

                foreach (Tag tag in _tagList) _XML.Add(tag.XML);
                foreach (TagGroup tag in _tagGroupList) _XML.Add(tag.XML);

                return _XML;
            }
        }

        private XElement createTag()
        {
            XElement newChannel = new XElement("Channel");
            XAttribute name = new XAttribute("Name", _name);
            XAttribute no = new XAttribute("No", _no);
            XAttribute kind = new XAttribute("Kind", _kind);
            XAttribute prefix = new XAttribute("Prefix", _prefix);

            newChannel.Add(no, name, kind, prefix);

            return newChannel;
        }

        public void AddTagGroup(TagGroup tagGroup)
        {
            _tagGroupList.Add(tagGroup);
        }

        public void AddTag(Tag tag)
        {
            _tagList.Add(tag);
        }

        public Tag GetTagByOrdinal(int ordinal)
        {
            foreach (Tag tag in _tagList)
            {
                if (tag.Ordinal == ordinal) return tag;
            }

            return null;
        }

        public TagGroup GetTagGroupByOrdinal(int ordinal)
        {
            foreach (TagGroup tag in _tagGroupList)
            {
                if (tag.Ordinal == ordinal) return tag;
            }

            return null;
        }

        public AbstractTag GetTagById(string id)
        {
            foreach (Tag tag in _tagList)
            {
                if (tag.Id == id) return tag;
            }

            foreach (TagGroup tagGroup in _tagGroupList)
            {
                GroupTag result = tagGroup.GetGroupTagById(id);
                if (result != null) return result;
            }

            return null;
        }

    }
}

using System.Xml.Linq;

namespace Multifunctional_heat_meters_gui.DB
{
    abstract class AbstractTag
    {
        private string _name;
        private string _value;
        private string _id;
        private string _EU;
        private XElement _XML;

        public string Name
        {
            get { return _name; }
        }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string Id
        {
            get { return _id; }
        }
        public string EU
        {
            get { return _EU; }
        }
        public XElement XML
        {
            get { return _XML; }
        }

        private XElement createTag()
        {
            XElement newTag = new XElement("Tag");

            XAttribute name = new XAttribute("Name", _name);
            XAttribute id = new XAttribute("Id", _id);
            XAttribute value = new XAttribute("Value", _value);
            XAttribute eu = new XAttribute("Eu", _EU);

            newTag.Add(name, id, value, eu);

            return newTag;
        }


        public AbstractTag(string id, string value = "нет данных???", string name = "", string eu = " ")
        {
            _name = name;
            _value = value;
            _id = id;
            _EU = eu;
            _XML = createTag();

        }


    }
}

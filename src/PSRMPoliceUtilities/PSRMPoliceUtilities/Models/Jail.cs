using System.Xml.Serialization;

namespace PSRMPoliceUtilities.Models
{
    public class Jail
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public float X { get; set; }

        [XmlAttribute]
        public float Y { get; set; }

        [XmlAttribute]
        public float Z { get; set; }
    }
}
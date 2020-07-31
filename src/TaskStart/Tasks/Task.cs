using System;
using System.Xml.Serialization;

namespace TaskStart.Tasks
{
    [Serializable]
    public class Task
    {
        [XmlAttribute]
        public string ApplicationPath { get; set; }

        [XmlAttribute]
        public string ApplicationArguments { get; set; }


        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Category { get; set; }
    }
}
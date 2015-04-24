using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.DalConfig
{
    public class DalSqlNodes
    {
        [XmlElement("SQLText")]
        public List<SQLText> SQLTexts { get; set; }
    }

    public class SQLText
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}

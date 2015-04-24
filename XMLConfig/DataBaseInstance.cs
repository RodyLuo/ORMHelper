using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    [XmlRoot("connectionStrings")]
    public class DatabaseInstance
    {
        private string m_Name;
        private string m_ConnectionString;

        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        [XmlElement("connectionString")]
        public string ConnectionString
        {
            get
            {
                return this.m_ConnectionString;
            }
            set
            {
                this.m_ConnectionString = value;
            }
        }
    }
}

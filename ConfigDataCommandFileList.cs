using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [XmlRoot("dataCommandFiles", Namespace = "http://process.flight.sh.ctriptravel.com/DbCommandFiles")]
    public class ConfigDataCommandFileList
    {
        private ConfigDataCommandFileList.DataCommandFile[] m_commandFiles;

        [XmlElement("file")]
        public ConfigDataCommandFileList.DataCommandFile[] FileList
        {
            get
            {
                return this.m_commandFiles;
            }
            set
            {
                this.m_commandFiles = value;
            }
        }

        public class DataCommandFile
        {
            private string m_FileName;

            [XmlAttribute("name")]
            public string FileName
            {
                get
                {
                    return this.m_FileName;
                }
                set
                {
                    this.m_FileName = value;
                }
            }
        }
    }
}

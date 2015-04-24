using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    [XmlRoot("configFileList")]
    public class ConfigurationFileList
    {
        private List<ConfigurationFile> _configurationList;

        [XmlElement("configFile")]
        public List<ConfigurationFile> ConfigurationList
        {
            get
            {
                return this._configurationList;
            }
            set
            {
                this._configurationList = value;
            }
        }
    }
}

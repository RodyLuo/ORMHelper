using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class ConfigurationFile
    {
        private bool _isAbsolute = false;
        private string _kye;
        private string _path;

        [XmlAttribute("key")]
        public string Key
        {
            get
            {
                return this._kye;
            }
            set
            {
                this._kye = value;
            }
        }

        [XmlAttribute("path")]
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        [XmlAttribute("isAbsolute")]
        public bool IsAbsolute
        {
            get
            {
                return this._isAbsolute;
            }
            set
            {
                this._isAbsolute = value;
            }
        }
    }
}

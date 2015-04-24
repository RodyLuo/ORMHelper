using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [XmlRoot(IsNullable = false, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [GeneratedCode("xsd", "2.0.50727.42")]
    [DebuggerStepThrough]
    [Serializable]
    public class dataOperations
    {
        private dataOperationsDataCommand[] dataCommandField;

        [XmlElement("dataCommand")]
        public dataOperationsDataCommand[] dataCommand
        {
            get
            {
                return this.dataCommandField;
            }
            set
            {
                this.dataCommandField = value;
            }
        }

        public IList<string> GetCommandNames()
        {
            if (this.dataCommandField == null || this.dataCommandField.Length == 0)
                return (IList<string>)new string[0];
            List<string> list = new List<string>(this.dataCommandField.Length);
            for (int index = 0; index < this.dataCommandField.Length; ++index)
                list.Add(this.dataCommandField[index].name);
            return (IList<string>)list;
        }
    }
}

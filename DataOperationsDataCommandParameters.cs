using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [GeneratedCode("xsd", "2.0.50727.42")]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [DebuggerStepThrough]
    [Serializable]
    public class dataOperationsDataCommandParameters
    {
        private dataOperationsDataCommandParametersParam[] paramField;

        [XmlElement("param")]
        public dataOperationsDataCommandParametersParam[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }
    }
}

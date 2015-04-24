using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [GeneratedCode("xsd", "2.0.50727.42")]
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [Serializable]
    public enum dataOperationsDataCommandParametersParamDirection
    {
        Input,
        InputOutput,
        Output,
        ReturnValue,
    }
}

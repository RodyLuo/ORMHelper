using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [GeneratedCode("xsd", "2.0.50727.42")]
    [Serializable]
    public enum dataOperationsDataCommandParametersParamDbType
    {
        AnsiString,
        Binary,
        Boolean,
        Byte,
        Currency,
        Date,
        DateTime,
        Decimal,
        Double,
        Int16,
        Int32,
        Int64,
        SByte,
        Single,
        String,
        StringFixedLength,
        AnsiStringFixedLength,
        Time,
        UInt16,
        UInt32,
        UInt64,
        VarNumeric,
        Xml,
        Object,
    }
}

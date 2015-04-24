using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [GeneratedCode("xsd", "2.0.50727.42")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [Serializable]
    public class dataOperationsDataCommandParametersParam
    {
        private string nameField;
        private dataOperationsDataCommandParametersParamDbType dbTypeField;
        private dataOperationsDataCommandParametersParamDirection directionField;
        private int sizeField;

        [XmlAttribute]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [XmlAttribute]
        public dataOperationsDataCommandParametersParamDbType dbType
        {
            get
            {
                return this.dbTypeField;
            }
            set
            {
                this.dbTypeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(dataOperationsDataCommandParametersParamDirection.Input)]
        public dataOperationsDataCommandParametersParamDirection direction
        {
            get
            {
                return this.directionField;
            }
            set
            {
                this.directionField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(-1)]
        public int size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        public dataOperationsDataCommandParametersParam()
        {
            this.directionField = dataOperationsDataCommandParametersParamDirection.Input;
            this.sizeField = -1;
        }

        public DbParameter GetDbParameter()
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = this.name;
            sqlParameter.Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), ((object)this.direction).ToString());
            if (this.size != -1)
                sqlParameter.Size = this.size;
            DbType dbType = (DbType)Enum.Parse(typeof(DbType), ((object)this.dbType).ToString());
            sqlParameter.DbType = dbType;
            return (DbParameter)sqlParameter;
        }
    }
}

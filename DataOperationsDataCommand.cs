using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    [XmlType(AnonymousType = true, Namespace = "http://process.flight.sh.ctriptravel.com/DataOperation")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("xsd", "2.0.50727.42")]
    [Serializable]
    public class dataOperationsDataCommand
    {
        private string commandTextField;
        private dataOperationsDataCommandParameters parametersField;
        private string nameField;
        private string databaseField;
        private bool databaseFieldSpecified;
        private dataOperationsDataCommandCommandType commandTypeField;
        private int timeOutField;

        public string commandText
        {
            get
            {
                return this.commandTextField;
            }
            set
            {
                this.commandTextField = value;
            }
        }

        public dataOperationsDataCommandParameters parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }

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
        public string database
        {
            get
            {
                return this.databaseField;
            }
            set
            {
                this.databaseField = value;
            }
        }

        [XmlIgnore]
        public bool databaseSpecified
        {
            get
            {
                return this.databaseFieldSpecified;
            }
            set
            {
                this.databaseFieldSpecified = value;
            }
        }

        [DefaultValue(dataOperationsDataCommandCommandType.Text)]
        [XmlAttribute]
        public dataOperationsDataCommandCommandType commandType
        {
            get
            {
                return this.commandTypeField;
            }
            set
            {
                this.commandTypeField = value;
            }
        }

        [XmlAttribute]
        [DefaultValue(300)]
        public int timeOut
        {
            get
            {
                return this.timeOutField;
            }
            set
            {
                this.timeOutField = value;
            }
        }

        public dataOperationsDataCommand()
        {
            this.commandTypeField = dataOperationsDataCommandCommandType.Text;
            this.timeOutField = 300;
        }

        public DataCommand GetDataCommand()
        {
            return new DataCommand(((object)this.database).ToString(), this.GetDbCommand());
        }

        private DbCommand GetDbCommand()
        {
            DbCommand dbCommand = DbCommandFactory.CreateDbCommand();
            dbCommand.CommandText = this.commandText.Trim();
            dbCommand.CommandTimeout = this.timeOut;
            dbCommand.CommandType = (CommandType)Enum.Parse(typeof(CommandType), ((object)this.commandType).ToString());
            if (this.parameters != null && this.parameters.param != null && this.parameters.param.Length > 0)
            {
                foreach (dataOperationsDataCommandParametersParam commandParametersParam in this.parameters.param)
                    dbCommand.Parameters.Add((object)commandParametersParam.GetDbParameter());
            }
            return dbCommand;
        }
    }
}

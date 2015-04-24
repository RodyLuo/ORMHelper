using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public class CustomDataCommand : DataCommand
    {
        public CommandType CommandType
        {
            get
            {
                return this.m_DbCommand.CommandType;
            }
            set
            {
                this.m_DbCommand.CommandType = value;
            }
        }

        public string CommandText
        {
            get
            {
                return this.m_DbCommand.CommandText;
            }
            set
            {
                this.m_DbCommand.CommandText = value;
            }
        }

        public new int CommandTimeout
        {
            get
            {
                return this.m_DbCommand.CommandTimeout;
            }
            set
            {
                this.m_DbCommand.CommandTimeout = value;
            }
        }

        public string DatabaseAliasName
        {
            get
            {
                return this.m_DatabaseName;
            }
            set
            {
                this.m_DatabaseName = ((object)value).ToString();
            }
        }

        internal CustomDataCommand(string databaseAliasName)
            : base(databaseAliasName, DbCommandFactory.CreateDbCommand())
        {
        }

        internal CustomDataCommand(string databaseAliasName, CommandType commandType)
            : this(databaseAliasName)
        {
            this.CommandType = commandType;
        }

        internal CustomDataCommand(string databaseAliasName, CommandType commandType, string commandText)
            : this(databaseAliasName, commandType)
        {
            this.CommandText = commandText;
        }

        public void AddInputParameter(string name, DbType dbType, object value)
        {
            object obj = value;
            if (value == null)
                obj = (object)DBNull.Value;
            this.ActualDatabase.AddInParameter(this.m_DbCommand, name, dbType, obj);
        }

        public void AddInputParameter(string name, DbType dbType)
        {
            this.ActualDatabase.AddInParameter(this.m_DbCommand, name, dbType);
        }

        public void AddOutParameter(string name, DbType dbType, int size)
        {
            this.ActualDatabase.AddOutParameter(this.m_DbCommand, name, dbType, size);
        }
    }
}

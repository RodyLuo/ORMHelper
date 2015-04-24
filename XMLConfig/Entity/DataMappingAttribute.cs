using System;
using System.Data;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity
{
    public class DataMappingAttribute : Attribute
    {
        private string m_ColumnName;
        private DbType m_DbType;

        public string ColumnName
        {
            get
            {
                return this.m_ColumnName;
            }
        }

        public DbType DbType
        {
            get
            {
                return this.m_DbType;
            }
        }

        public DataMappingAttribute(string columnName, DbType dbType)
        {
            this.m_ColumnName = columnName;
            this.m_DbType = dbType;
        }
    }
}

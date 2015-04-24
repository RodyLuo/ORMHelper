using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity
{
    internal class DataRowEntitySource : IEntityDataSource, IEnumerable<string>, IEnumerable, IDisposable
    {
        private DataRow m_DataRow;

        public object this[string columnName]
        {
            get
            {
                return this.m_DataRow[columnName];
            }
        }

        public object this[int index]
        {
            get
            {
                return this.m_DataRow[index];
            }
        }

        public DataRowEntitySource(DataRow dr)
        {
            this.m_DataRow = dr;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (IEnumerator<string>)new DataRowEntitySource.RowColumnNameEnumerator(this.m_DataRow);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public bool ContainsColumn(string columnName)
        {
            return this.m_DataRow.Table.Columns.Contains(columnName);
        }

        public void Dispose()
        {
        }

        private class RowColumnNameEnumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private IEnumerator m_InternalEnumeator;

            public string Current
            {
                get
                {
                    return (this.m_InternalEnumeator.Current as DataColumn).ColumnName;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (object)this.Current;
                }
            }

            public RowColumnNameEnumerator(DataRow dr)
            {
                this.m_InternalEnumeator = dr.Table.Columns.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.m_InternalEnumeator.MoveNext();
            }

            public void Reset()
            {
                this.m_InternalEnumeator.Reset();
            }

            public void Dispose()
            {
            }
        }
    }
}

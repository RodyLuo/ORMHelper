using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity
{
    internal class DataReaderEntitySource : IEntityDataSource, IEnumerable<string>, IEnumerable, IDisposable
    {
        private IDataReader m_DataReader;

        public object this[string columnName]
        {
            get
            {
                return this.m_DataReader[columnName];
            }
        }

        public object this[int index]
        {
            get
            {
                return this.m_DataReader[index];
            }
        }

        public DataReaderEntitySource(IDataReader dr)
        {
            this.m_DataReader = dr;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (IEnumerator<string>)new DataReaderEntitySource.ReaderColumnNameEnumerator(this.m_DataReader);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)new DataReaderEntitySource.ReaderColumnNameEnumerator(this.m_DataReader);
        }

        public bool ContainsColumn(string columnName)
        {
            foreach (DataRow dataRow in (InternalDataCollectionBase)this.m_DataReader.GetSchemaTable().Rows)
            {
                if (string.Compare(dataRow["ColumnName"].ToString().Trim(), columnName.Trim(), true) == 0)
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (this.m_DataReader == null)
                return;
            this.m_DataReader.Dispose();
        }

        private class ReaderColumnNameEnumerator : IEnumerator<string>, IDisposable, IEnumerator
        {
            private IEnumerator m_InternalEnumerator;

            public string Current
            {
                get
                {
                    return (this.m_InternalEnumerator.Current as DataRow)["ColumnName"] as string;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (object)this.Current;
                }
            }

            public ReaderColumnNameEnumerator(IDataReader dr)
            {
                this.m_InternalEnumerator = dr.GetSchemaTable().Rows.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.m_InternalEnumerator.MoveNext();
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                this.m_InternalEnumerator.Reset();
            }
        }
    }
}

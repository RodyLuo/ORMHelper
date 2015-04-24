using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public class DataCommand : ICloneable
    {
        protected DbCommand m_DbCommand;
        protected string m_DatabaseName;

        public int CommandTimeout
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

        protected Database ActualDatabase
        {
            get
            {
                //return DatabaseManager.GetDatabase(this.m_DatabaseName);
                return DatabaseFactory.CreateDatabase(this.m_DatabaseName);
                //m_DatabaseName
            }
        }

        internal DataCommand(string databaseName, DbCommand command)
        {
            this.m_DatabaseName = databaseName;
            this.m_DbCommand = command;
        }

        private DataCommand()
        {
        }

        public object Clone()
        {
            DataCommand dataCommand = new DataCommand();
            if (this.m_DbCommand != null)
            {
                if (!(this.m_DbCommand is ICloneable))
                    throw new ApplicationException("A class that implements IClonable is expected.");
                dataCommand.m_DbCommand = ((ICloneable)this.m_DbCommand).Clone() as DbCommand;
            }
            dataCommand.m_DatabaseName = this.m_DatabaseName;
            return (object)dataCommand;
        }

        public object GetParameterValue(string paramName)
        {
            return this.ActualDatabase.GetParameterValue(this.m_DbCommand, paramName);
        }

        public void SetParameterValue(string paramName, object val)
        {
            object obj = val;
            if (obj == null)
                obj = (object)DBNull.Value;
            else if (obj.ToString() == new DateTime().ToString())
                obj = (object)DBNull.Value;
            this.ActualDatabase.SetParameterValue(this.m_DbCommand, paramName, obj);
        }

        public void ReplaceParameterValue(string paramName, string paramValue)
        {
            if (this.m_DbCommand == null)
                return;
            this.m_DbCommand.CommandText = this.m_DbCommand.CommandText.Replace(paramName, paramValue);
        }

        public T ExecuteScalar<T>()
        {
            try
            {
                return this.Retry<T>((DataCommand.RetryHandler<T>)(() => (T)this.ActualDatabase.ExecuteScalar(this.m_DbCommand)));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
        }

        public object ExecuteScalar()
        {
            try
            {
                return this.Retry<object>((DataCommand.RetryHandler<object>)(() => this.ActualDatabase.ExecuteScalar(this.m_DbCommand)));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
        }

        public int ExecuteNonQuery()
        {
            try
            {
                return this.Retry<int>((DataCommand.RetryHandler<int>)(() => this.ActualDatabase.ExecuteNonQuery(this.m_DbCommand)));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
        }

        public T ExecuteEntity<T>() where T : class, new()
        {
            IDataReader reader = (IDataReader)null;
            try
            {
                return this.Retry<T>((DataCommand.RetryHandler<T>)(() =>
                {
                    if (reader != null)
                        reader.Close();
                    reader = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                    if (reader.Read())
                        return EntityBuilder.BuildEntity<T>(reader);
                    else
                        return default(T);
                }));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        public List<T> ExecuteEntityList<T>() where T : class, new()
        {
            IDataReader reader = (IDataReader)null;
            try
            {
                return this.Retry<List<T>>((DataCommand.RetryHandler<List<T>>)(() =>
                {
                    if (reader != null)
                        reader.Close();
                    reader = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                    List<T> list = new List<T>();
                    while (reader.Read())
                    {
                        T obj = EntityBuilder.BuildEntity<T>(reader);
                        list.Add(obj);
                    }
                    return list;
                }));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        public K ExecuteEntityCollection<T, K>()
            where T : class, new()
            where K : ICollection<T>, new()
        {
            IDataReader reader = (IDataReader)null;
            try
            {
                return this.Retry<K>((DataCommand.RetryHandler<K>)(() =>
                {
                    if (reader != null)
                        reader.Close();
                    reader = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                    ICollection<T> collection = (ICollection<T>)new K();
                    while (reader.Read())
                    {
                        T obj = EntityBuilder.BuildEntity<T>(reader);
                        collection.Add(obj);
                    }
                    return (K)collection;
                }));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        public IDataReader ExecuteDataReader()
        {
            try
            {
                return this.Retry<IDataReader>((DataCommand.RetryHandler<IDataReader>)(() => this.ActualDatabase.ExecuteReader(this.m_DbCommand)));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
        }

        public DataSet ExecuteDataSet()
        {
            try
            {
                return this.Retry<DataSet>((DataCommand.RetryHandler<DataSet>)(() => this.ActualDatabase.ExecuteDataSet(this.m_DbCommand)));
            }
            catch (Exception ex)
            {
                throw DataAccessLogger.LogExecutionError(this.m_DbCommand, ex);
            }
        }

        private T Retry<T>(DataCommand.RetryHandler<T> function)
        {
            if (function == null)
                throw new ArgumentNullException("function can not be null.");
            int num1 = 5;
            double num2 = 0.1;
            T obj = default(T);
            for (int index = 0; index < num1; ++index)
            {
                try
                {
                    obj = function();
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 10054 || ex.Number == 10053)
                    {
                        if (index == num1 - 1)
                        {
                            throw;
                        }
                        else
                        {
                            SqlConnection.ClearAllPools();
                            Thread.Sleep(TimeSpan.FromSeconds(num2));
                        }
                    }
                    else
                        throw;
                }
            }
            return obj;
        }

        private delegate T RetryHandler<T>();
    }
}

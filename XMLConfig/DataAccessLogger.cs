using System;
using System.Data.Common;
using System.IO;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal static class DataAccessLogger
    {
        private const string LOG_CATEGORY_NAME = "Ctrip.Flight.OrderProcess.DataBaseHelper";
        private const int LoadDatabaseFile = 1;
        private const int LoadCommandInventoryFile = 2;
        private const int LoadCommandFile = 3;
        private const int DBFileChanged = 10;
        private const int EXECUTION_ERROR = 20;

        public static void LogDatabaseFileChanged(FileSystemEventArgs arg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("File name: " + arg.FullPath + Environment.NewLine);
            stringBuilder.Append("Change type: " + ((object)arg.ChangeType).ToString());
        }

        public static Exception LogDatabaseFileLoaded(string fileName, Exception sourceException)
        {
            return new Exception("Database config file loaded: " + fileName, sourceException);
        }

        public static Exception LogDatabaseCommandFileLoaded(string fileName, Exception sourceException)
        {
            return new Exception("Data command file loaded: " + fileName, sourceException);
        }

        public static Exception LogDataCommandInventoryFileLoaded(string fileName, int count, Exception sourceException)
        {
            return new Exception("Data command inventory file loaded: " + fileName + ". " + count.ToString() + " command file(s) found.", sourceException);
        }

        public static Exception LogExecutionError(DbCommand cmd, Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DataCommand Execution error, command text:");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(cmd.CommandText);
            stringBuilder.Append(Environment.NewLine);
            if (cmd != null)
            {
                stringBuilder.Append("command parameters inforamtion:");
                stringBuilder.Append(Environment.NewLine);
                for (int index = 0; index < cmd.Parameters.Count; ++index)
                {
                    stringBuilder.AppendFormat("parameters name:{0}, parameters value:{1}, parameters type:{2}", (object)cmd.Parameters[index].ParameterName, cmd.Parameters[index].Value, (object)cmd.Parameters[index].DbType);
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Exception: ");
            stringBuilder.Append(ex.Message);
            return new Exception(((object)stringBuilder).ToString(), ex);
        }

        private static void LogEvent(int eventId, string message)
        {
        }
    }
}

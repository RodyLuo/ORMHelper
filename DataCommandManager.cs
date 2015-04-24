using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public static class DataCommandManager
    {
        private static ConfigIntanceBase _commonConfigInstance = DbInstanceManager.CreateDbInance();
        private static FileSystemChangeEventHandler s_FileChangeHandler = new FileSystemChangeEventHandler(500);
        private const string EventCategory = "DataCommandManager";
        private const int FILE_CHANGE_NOTIFICATION_INTERVAL = 500;
        private static object s_CommandSyncObject;
        private static object s_CommandFileListSyncObject;
        private static FileSystemWatcher s_Watcher;
        private static Dictionary<string, DataCommand> s_DataCommands;
        private static string s_DataFileFolder;
        private static Dictionary<string, IList<string>> s_FileCommands;
        private static Dictionary<string, dataOperationsDataCommand> s_dataConfigurationData;

        private static string DataCommandListFileName
        {
            get
            {
                return Path.GetFileName(DataCommandManager._commonConfigInstance.DataCommandFileListConfigFile);
            }
        }

        static DataCommandManager()
        {
            DataCommandManager.s_FileChangeHandler.ActualHandler += new FileSystemEventHandler(DataCommandManager.Watcher_Changed);
            DataCommandManager.s_DataFileFolder = Path.GetDirectoryName(DataCommandManager._commonConfigInstance.DataCommandFileListConfigFile);
            DataCommandManager.s_CommandSyncObject = new object();
            DataCommandManager.s_CommandFileListSyncObject = new object();
            if (DataCommandManager.s_DataFileFolder == null || !Directory.Exists(DataCommandManager.s_DataFileFolder))
                return;
            DataCommandManager.s_Watcher = new FileSystemWatcher(DataCommandManager.s_DataFileFolder);
            DataCommandManager.s_Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            DataCommandManager.s_Watcher.Changed += new FileSystemEventHandler(DataCommandManager.s_FileChangeHandler.ChangeEventHandler);
            DataCommandManager.s_Watcher.EnableRaisingEvents = true;
            DataCommandManager.UpdateAllCommandFiles();
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (string.Compare(e.FullPath, DataCommandManager._commonConfigInstance.DataCommandFileListConfigFile, true) == 0)
            {
                DataCommandManager.UpdateAllCommandFiles();
            }
            else
            {
                lock (DataCommandManager.s_CommandFileListSyncObject)
                {
                    foreach (string item_0 in DataCommandManager.s_FileCommands.Keys)
                    {
                        if (string.Compare(item_0, e.FullPath, true) == 0)
                        {
                            DataCommandManager.UpdateCommandFile(item_0);
                            break;
                        }
                    }
                }
            }
        }

        private static void UpdateCommandFile(string fileName)
        {
            IList<string> list = !DataCommandManager.s_FileCommands.ContainsKey(fileName) ? (IList<string>)null : DataCommandManager.s_FileCommands[fileName];
            lock (DataCommandManager.s_CommandSyncObject)
            {
                Dictionary<string, DataCommand> local_1 = new Dictionary<string, DataCommand>((IDictionary<string, DataCommand>)DataCommandManager.s_DataCommands);
                if (list != null)
                {
                    foreach (string item_1 in (IEnumerable<string>)list)
                        local_1.Remove(item_1);
                }
                dataOperations local_3 = Utilities.LoadFromXml<dataOperations>(fileName);
                if (local_3 == null)
                    throw new DataCommandFileLoadException(fileName);
                if (local_3.dataCommand != null && local_3.dataCommand.Length > 0)
                {
                    foreach (dataOperationsDataCommand item_0 in local_3.dataCommand)
                    {
                        try
                        {
                            local_1.Add(item_0.name, item_0.GetDataCommand());
                            if (DataCommandManager.s_dataConfigurationData.ContainsKey(item_0.name))
                                DataCommandManager.s_dataConfigurationData[item_0.name] = item_0;
                            else
                                DataCommandManager.s_dataConfigurationData.Add(item_0.name, item_0);
                        }
                        catch (Exception exception_0)
                        {
                            throw new Exception("Command:" + item_0.name + " has exists.", exception_0);
                        }
                    }
                    DataCommandManager.s_DataCommands = local_1;
                }
                DataCommandManager.s_FileCommands[fileName] = local_3.GetCommandNames();
            }
        }

        private static void UpdateAllCommandFiles()
        {
            lock (DataCommandManager.s_CommandFileListSyncObject)
            {
                ConfigDataCommandFileList local_0 = Utilities.LoadFromXml<ConfigDataCommandFileList>(DataCommandManager._commonConfigInstance.DataCommandFileListConfigFile);
                if (local_0 == null || local_0.FileList == null)
                    return;
                DataCommandManager.s_FileCommands = new Dictionary<string, IList<string>>();
                DataCommandManager.s_DataCommands = new Dictionary<string, DataCommand>();
                DataCommandManager.s_dataConfigurationData = new Dictionary<string, dataOperationsDataCommand>();
                foreach (ConfigDataCommandFileList.DataCommandFile item_0 in local_0.FileList)
                {
                    string local_2 = Path.GetPathRoot(item_0.FileName);
                    string local_3 = string.Empty;
                    DataCommandManager.UpdateCommandFile(local_2 == string.Empty || local_2.StartsWith("\\") ? Path.Combine(DataCommandManager.s_DataFileFolder, item_0.FileName) : item_0.FileName);
                }
            }
        }

        public static DataCommand GetDataCommand(string name)
        {
            return DataCommandManager.s_DataCommands[name].Clone() as DataCommand;
        }

        public static CustomDataCommand CreateCustomDataCommand(string databaseAliasName)
        {
            return new CustomDataCommand(databaseAliasName);
        }

        public static CustomDataCommand CreateCustomDataCommand(string databaseAliasName, CommandType commandType)
        {
            return new CustomDataCommand(databaseAliasName, commandType);
        }

        public static CustomDataCommand CreateCustomDataCommand(string databaseAliasName, CommandType commandType, string commandText)
        {
            return new CustomDataCommand(databaseAliasName, commandType, commandText);
        }

        public static CustomDataCommand CreateCustomDataCommandFromConfig(string sqlNameInConfig)
        {
            dataOperationsDataCommand operationsDataCommand = DataCommandManager.s_dataConfigurationData[sqlNameInConfig];
            if (operationsDataCommand == null)
                throw new ApplicationException("Can not find any configuration match the input name in SQL configuration files.");
            CommandType commandType = CommandType.Text;
            switch (operationsDataCommand.commandType)
            {
                case dataOperationsDataCommandCommandType.StoredProcedure:
                    commandType = CommandType.StoredProcedure;
                    break;
                case dataOperationsDataCommandCommandType.TableDirect:
                    commandType = CommandType.TableDirect;
                    break;
                case dataOperationsDataCommandCommandType.Text:
                    commandType = CommandType.Text;
                    break;
            }
            return new CustomDataCommand(operationsDataCommand.database, commandType, operationsDataCommand.commandText);
        }
    }
}

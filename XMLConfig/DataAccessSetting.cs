using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal static class DataAccessSetting
    {

        internal static readonly string DataCommandFileConfigName = "DataCommandFile";
        private static string s_DataCommandFileListConfigFile;

        public static string DataCommandFileListConfigFile
        {
            get
            {
                return DataAccessSetting.s_DataCommandFileListConfigFile;
            }
        }

        static DataAccessSetting()
        {
            if (OrderProcessConfig.LocalDataAccessManager != null && !string.IsNullOrEmpty(OrderProcessConfig.LocalDataAccessManager.DataCommandFile))
                DataAccessSetting.s_DataCommandFileListConfigFile = Utilities.GetFileFullPath(OrderProcessConfig.LocalDataAccessManager.DataCommandFile);
        }
    }
}

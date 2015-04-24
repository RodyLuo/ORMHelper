using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class DbInstanceManager
    {
        public static ConfigIntanceBase CreateDbInance()
        {
            CommonDataInstance instance = new CommonDataInstance(
                                    DataAccessSetting.DataCommandFileListConfigFile == null ?
                                        string.Empty : Utilities.GetFileFullPath(@"SQLConfig\DbCommandFiles.xml"));   
            return instance;
        }
    }
}

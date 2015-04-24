using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class CommonDataInstance : ConfigIntanceBase
    {
        private string _dbConfigFile = string.Empty;
        private string _dataCommandFileListConfigFile = DataAccessSetting.DataCommandFileListConfigFile;
                
        public override string DataCommandFileListConfigFile
        {
            get
            {
                return this._dataCommandFileListConfigFile;
            }
        }

        public CommonDataInstance(string dataCommandFileListConfigFile)
        {
            this._dataCommandFileListConfigFile = dataCommandFileListConfigFile;
        }
    }
}

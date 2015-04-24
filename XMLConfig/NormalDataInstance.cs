using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class NormalDataInstance : ConfigIntanceBase
    {
        private string _dataCommandFileListConfigFile = DataAccessSetting.DataCommandFileListConfigFile;

        public override string DataCommandFileListConfigFile
        {
            get
            {
                return this._dataCommandFileListConfigFile;
            }
        }
    }
}

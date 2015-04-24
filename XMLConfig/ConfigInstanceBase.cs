using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public abstract class ConfigIntanceBase
    {
        public abstract string DatabaseConfigFile { get; }

        public abstract string DataCommandFileListConfigFile { get; }

        public IList<DatabaseInstance> GetIntanceList()
        {
            IList<DatabaseInstance> list = (IList<DatabaseInstance>)null;
            DatabaseList databaseList = Utilities.LoadFromXml<DatabaseList>(this.DatabaseConfigFile);
            if (databaseList != null && databaseList.DatabaseInstances != null && databaseList.DatabaseInstances.Length != 0)
            {
                list = (IList<DatabaseInstance>)new List<DatabaseInstance>((IEnumerable<DatabaseInstance>)databaseList.DatabaseInstances);
                foreach (DatabaseInstance databaseInstance in (IEnumerable<DatabaseInstance>)list)
                    databaseInstance.ConnectionString = Utilities.Decrypt(databaseInstance.ConnectionString);
            }
            return list;
        }
    }
}

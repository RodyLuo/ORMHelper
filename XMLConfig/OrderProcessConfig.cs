using System.Collections.Specialized;
using System.Configuration;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public static class OrderProcessConfig
    {
        public static DataAccessManagerSection LocalDataAccessManager
        {
            get
            {
                return new LocalConfigurationSource().GetSection<DataAccessManagerSection>("orderprocess/dataAccessSettings");
            }
        }
    }
}

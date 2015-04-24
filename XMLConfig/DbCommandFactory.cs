using System.Data.Common;
using System.Data.SqlClient;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    internal static class DbCommandFactory
    {
        public static DbCommand CreateDbCommand()
        {
            return (DbCommand)new SqlCommand();
        }
    }
}

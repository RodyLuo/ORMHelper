using System.Configuration;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class LocalConfigurationSource : IConfigurationSource
    {
        public T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}

using System.Configuration;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public interface IConfigurationSource
    {
        T GetSection<T>(string sectionName) where T : ConfigurationSection;
    }
}

using System.Configuration;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public sealed class DataAccessManagerSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty propDataCommandFile = new ConfigurationProperty("DataCommandFile", typeof(string), (object)null, ConfigurationPropertyOptions.IsRequired);
        protected static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        [ConfigurationProperty("DataCommandFile")]
        public string DataCommandFile
        {
            get
            {
                return this[DataAccessManagerSection.propDataCommandFile] as string;
            }
            set
            {
                this[DataAccessManagerSection.propDataCommandFile] = (object)value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return DataAccessManagerSection.properties;
            }
        }

        static DataAccessManagerSection()
        {
            DataAccessManagerSection.properties.Add(DataAccessManagerSection.propDataCommandFile);
        }
    }
}

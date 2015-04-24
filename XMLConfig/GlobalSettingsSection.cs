using System.Configuration;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public sealed class GlobalSettingsSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty propFilePath = new ConfigurationProperty("filePath", typeof(string));
        private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return GlobalSettingsSection.properties;
            }
        }

        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return (string)this[GlobalSettingsSection.propFilePath];
            }
            set
            {
                this[GlobalSettingsSection.propFilePath] = (object)value;
            }
        }

        static GlobalSettingsSection()
        {
            GlobalSettingsSection.properties.Add(GlobalSettingsSection.propFilePath);
        }
    }
}

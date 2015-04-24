using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class GlobalConfigurationSource : IConfigurationSource
    {
        private static ExeConfigurationFileMap fileMap;
        private string configurationFilepath;

        private GlobalConfigurationSource(string configurationFilepath)
        {
            if (string.IsNullOrEmpty(configurationFilepath))
                throw new ArgumentException("configurationFilepath");
            this.configurationFilepath = GlobalConfigurationSource.RootConfigurationFilePath(configurationFilepath);
            if (!File.Exists(this.configurationFilepath))
                throw new FileNotFoundException(string.Format("The configuration file {0} could not be found.", (object)this.configurationFilepath));
        }

        public static GlobalConfigurationSource Create()
        {
            GlobalSettingsSection globalSettingsSection = (GlobalSettingsSection)ConfigurationManager.GetSection("OrderProcess/globalSettings");
            if (globalSettingsSection == null)
                throw new ConfigurationErrorsException("The global configuration is null, please check your config file, make sure tha OrderProcess/globalSettings is exists.");
            else
                return new GlobalConfigurationSource(globalSettingsSection.FilePath);
        }

        public T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            if (GlobalConfigurationSource.fileMap == null)
            {
                GlobalConfigurationSource.fileMap = new ExeConfigurationFileMap();
                GlobalConfigurationSource.fileMap.ExeConfigFilename = GlobalConfigurationSource.RootConfigurationFilePath(this.configurationFilepath);
            }
            return (T)(object)ConfigurationManager.OpenMappedExeConfiguration(GlobalConfigurationSource.fileMap, ConfigurationUserLevel.None).GetSection(sectionName);
        }

        private static string RootConfigurationFilePath(string configurationFile)
        {
            string str = (string)configurationFile.Clone();
            if (!Path.IsPathRooted(str))
                str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, str);
            return str;
        }
    }
}

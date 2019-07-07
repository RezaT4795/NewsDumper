using NewsDump.Lib.Util;
using Olive;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NewsDump.UI.Utils
{
    public static class Conf
    {
        public static IEnumerable<KeyValuePair<string, string>> GetAll() =>
            ConfigurationManager.AppSettings.AllKeys
            .Select(x => new KeyValuePair<string, string>
            (x, ConfigurationManager.AppSettings.Get(x)));

        public static string Get(string key) => ConfigurationManager.AppSettings[key];
        public static T Get<T>(string key) => ConfigurationManager.AppSettings[key].To<T>();


        public static void Set(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                EventBus.Notify("Error writing app settings", "Error");
            }
        }
    }
}

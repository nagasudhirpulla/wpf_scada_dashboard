using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WPFScadaDashboard
{
    public static class AppSettingsHelper
    {
        public static T GetSetting<T>(string key, T defaultValue = default(T)) where T : IConvertible
        {
            string val = ConfigurationManager.AppSettings[key] ?? "";
            T result = defaultValue;
            if (!string.IsNullOrEmpty(val))
            {
                T typeDefault = default(T);
                if (typeof(T) == typeof(String))
                {
                    typeDefault = (T)(object)String.Empty;
                }
                result = (T)Convert.ChangeType(val, typeDefault.GetTypeCode());
            }
            return result;
        }

        public static void SetSetting(string key, string value)
        {
            // http://yizeng.me/2013/08/31/update-appsettings-and-custom-configuration-sections-in-appconfig-at-runtime/

            // ConfigurationManager.AppSettings[key] = value;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

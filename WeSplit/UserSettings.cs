using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit
{
    public static class UserSettings
    {
        private static bool showSplash;

        public static bool ShowSplash { 
            get {
                return int.Parse(ConfigurationManager.AppSettings.Get("showSplash")) == 0 ? false : true;
            }
            set
            {
                showSplash = value;
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                settings["showSplash"].Value = value ? "1" : "0";
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
        }

        private static int perPage;

        public static int PerPage {
            get 
            {
                return int.Parse(ConfigurationManager.AppSettings.Get("perPage"));
            }
            set
            {
                perPage = value;
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                settings["perPage"].Value = value.ToString();
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            }
        }
    }
}

using Europa.Extensions;
using System;
using System.Configuration;

namespace Europa.Commons
{
    public static class ConfigurationWrapper
    {
        private static string LOG_DIRECTORY_KEY = "LogDirectory";
        private static string LOG_DIRECTORY_DEFAULT = @"C:\tmp\europa\logs\";

        public static string UtilFile
        {
            get
            {
                return "util.log";
            }
        }

        public static string LogDirectory
        {
            get
            {
                string directory = ConfigurationManager.AppSettings[LOG_DIRECTORY_KEY];
                if (string.IsNullOrEmpty(directory))
                {
                    ConfigurationManager.AppSettings[LOG_DIRECTORY_KEY] = LOG_DIRECTORY_DEFAULT;
                    directory = LOG_DIRECTORY_DEFAULT;
                }
                return directory;
            }
        }

        public static bool GetBoolProperty(string propertyName)
        {
            return Convert.ToBoolean(GetStringProperty(propertyName));
        }

        public static int GetIntProperty(string propertyName)
        {
            return Convert.ToInt32(GetStringProperty(propertyName));
        }


        public static string GetConnectionStringProperty(string connectionString)
        {
            string property = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            if (!property.IsNull())
            {
                return property;
            }
            throw new SettingsPropertyNotFoundException(connectionString);

        }

        public static string GetStringProperty(string propertyName)
        {
            string property = ConfigurationManager.AppSettings[propertyName];
            if (!property.IsNull())
            {
                return property;
            }
            throw new SettingsPropertyNotFoundException(propertyName);

        }

    }
}

using Serilog;
using System;
using System.Configuration;

namespace Gardient.SQL2Email.Helpers
{
    static class ConfigHelper
    {
        public static string ConnectionString => GetConfig("connectionString");
        public static string Query => GetConfig("query");
        public static string BodyTemplatePath => GetConfig("bodyTemplatePath");
        public static string SubjectTemplate => GetConfig("subjectTemplate");
        private static string GetConfig(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                Log.Error("Could not find {Key} in the application config", key);
                throw new ArgumentNullException(key);
            }
            return value;
        }
    }
}

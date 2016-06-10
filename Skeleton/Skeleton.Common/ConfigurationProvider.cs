using Skeleton.Abstraction;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Skeleton.Common
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public T GetConfigurationValue<T>(string key)
        {
            return GetConfigurationValue(key, default(T), true);
        }

        public string GetConfigurationValue(string key)
        {
            return GetConfigurationValue<string>(key);
        }

        public T GetConfigurationValue<T>(string key, T defaultValue)
        {
            return GetConfigurationValue(key, defaultValue, false);
        }

        public string GetConfigurationValue(string key, string defaultValue)
        {
            return GetConfigurationValue<string>(key, defaultValue);
        }

        private static T GetConfigurationValue<T>(string key, T defaultValue, bool throwException)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                if (throwException)
                    throw new KeyNotFoundException("Key " + key + " not found.");

                return defaultValue;
            }
            try
            {
                if (typeof(Enum).IsAssignableFrom(typeof(T)))
                    return (T) Enum.Parse(typeof(T), value);

                return (T) Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                if (throwException)
                    throw;

                return defaultValue;
            }
        }
    }
}
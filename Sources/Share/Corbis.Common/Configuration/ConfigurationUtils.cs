using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Corbis.Common.Configuration
{
    /// <summary>
    /// Contains utilities to work with configuration
    /// </summary>
    public class ConfigurationUtils
    {
        /// <summary>
        /// Gets default configuration section name
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <returns></returns>
        public static string GetDefaultSectionName(ConfigurationSection section)
        {
            return GetDefaultSectionName(section.GetType());
        }
        /// <summary>
        /// Gets default configuration section name
        /// </summary>
        /// <typeparam name="T">Configuration section type</typeparam>
        /// <returns></returns>
        public static string GetDefaultSectionName<T>() where T : ConfigurationSection
        {
            return GetDefaultSectionName(typeof(T));
        }
        /// <summary>
        /// Gets default configuration section name
        /// </summary>
        /// <param name="type">Configuration section type</param>
        /// <returns></returns>
        public static string GetDefaultSectionName(Type type)
        {
            if (!typeof(ConfigurationSection).IsAssignableFrom(type))
                throw new Exception(string.Format("Type '{0}' is not inherited from '{1}'", type.ToString(), typeof(ConfigurationSection).ToString()));

            var attribute = type.GetCustomAttributes(typeof(DefaultConfigSectionNameAttribute), false).SingleOrDefault() as DefaultConfigSectionNameAttribute;
            return attribute == null ? null : attribute.Name;
        }

        /// <summary>
        /// Gets configuration section name by its default name or specific name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Config section name. If it is null then default configuration name is used if it pointed for section</param>
        /// <returns></returns>
        public static T GetSection<T>(string sectionName = null, bool bThrowException = true) where T : ConfigurationSection
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = ConfigurationUtils.GetDefaultSectionName<T>();

                if (string.IsNullOrEmpty(sectionName))
                {
                    if (bThrowException)
                        throw new Exception("Section name is not pointed and configuration section type does not contain default section name");
                    else
                        return null;
                }
            }

            return (T)ConfigurationManager.GetSection(sectionName);
 
        }
    }
}

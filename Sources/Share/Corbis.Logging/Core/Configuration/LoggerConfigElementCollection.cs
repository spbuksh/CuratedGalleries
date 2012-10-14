using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Corbis.Logging.Core.Configuration
{
    [ConfigurationCollection(typeof(LoggerConfigElement))]
    public class LoggerConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as LoggerConfigElement).Name;
        }
    }
}

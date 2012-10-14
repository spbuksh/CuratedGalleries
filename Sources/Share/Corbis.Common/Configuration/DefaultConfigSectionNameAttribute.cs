using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common.Configuration
{
    /// <summary>
    /// Sets default name for configuration section
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultConfigSectionNameAttribute : Attribute
    {
        /// <summary>
        /// Configuration section default name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public DefaultConfigSectionNameAttribute() : base()
        { }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Configuration section default name</param>
        public DefaultConfigSectionNameAttribute(string name) : this()
        {
            this.Name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common.ObjectMapping.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingData
    {
        #region Class Constructors

        public MappingData(bool ignoreCase, IEnumerable<MappingDataItem> propertyMappingItems, IEnumerable<string> excludedProperties)
        {
            this.IgnoreCase = ignoreCase;

            if (!object.ReferenceEquals(propertyMappingItems, null))
                this.PropertiesMapping.AddRange(propertyMappingItems);

            if (!object.ReferenceEquals(excludedProperties, null))
                this.ExcludedProperties.AddRange(excludedProperties);
        }
        public MappingData() : this(true, null, null)
        { }
        public MappingData(bool ignoreCase) : this(ignoreCase, null, null)
        { }
        public MappingData(IEnumerable<MappingDataItem> propertyMappingItems) : this(true, propertyMappingItems, null)
        { }
        public MappingData(bool ignoreCase, IEnumerable<MappingDataItem> propertyMappingItems) : this(ignoreCase, propertyMappingItems, null)
        { }
        public MappingData(IEnumerable<string> excludedProperties) : this(true, null, excludedProperties)
        { }
        public MappingData(bool ignoreCase, IEnumerable<string> excludedProperties) : this(ignoreCase, null, excludedProperties)
        { }
        public MappingData(IEnumerable<MappingDataItem> propertyMappingItems, IEnumerable<string> excludedProperties) : this(true, propertyMappingItems, excludedProperties)
        { }

        #endregion Class Constructors

        /// <summary>
        /// You can specify is mapping case sensetive or not. By default this value is true. 
        /// You can override this value for some specific properties in PropertiesMapping list
        /// </summary>
        public bool IgnoreCase
        {
            get { return this.m_ignoreCase; }
            set { this.m_ignoreCase = value; }
        }
        private bool m_ignoreCase = true;

        /// <summary>
        /// Excluded properties from mapping
        /// </summary>
        public List<string> ExcludedProperties
        {
            get { return this.m_ExcludedProperties; }
        }
        private readonly List<string> m_ExcludedProperties = new List<string>();

        /// <summary>
        /// Property mapping data items
        /// </summary>
        public List<MappingDataItem> PropertiesMapping
        {
            get { return this.m_mapping; }
        }
        private readonly List<MappingDataItem> m_mapping = new List<MappingDataItem>();
    }
}

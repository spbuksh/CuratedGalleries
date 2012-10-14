using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common.ObjectMapping.Interface
{
    /// <summary>
    /// This class contains data how to map 
    /// </summary>
    public class MappingDataItem
    {
        public MappingDataItem()
        { }
        public MappingDataItem(string propertyFrom, string propertyTo = null, ActionHandler<object, object> converter = null, bool ignoreCase = true)
            : this()
        {
            this.PropertyFrom = propertyFrom;
            this.PropertyTo = string.IsNullOrEmpty(propertyTo) ? this.PropertyFrom : propertyTo;
            this.IgnoreCase = ignoreCase;
            this.Converter = converter;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreCase
        {
            get { return this.m_ignoreCase; }
            set { this.m_ignoreCase = value; }
        }
        private bool m_ignoreCase = true;

        /// <summary>
        /// Source property. This property is requiered
        /// </summary>
        public string PropertyFrom 
        {
            get { return this.m_PropertyFrom; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException();

                this.m_PropertyFrom = value;
            }
        }
        private string m_PropertyFrom = null;

        /// <summary>
        /// Target property. It is not required property. If you do not set it then the system will
        /// quess that target property name is equal to 'PropertyFrom'
        /// </summary>
        public string PropertyTo
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_propertyTo))
                    return this.PropertyFrom;

                return this.m_propertyTo;
            }
            set
            {
                this.m_propertyTo = value;
            }
        }
        private string m_propertyTo = null;

        /// <summary>
        /// You can specify logic of convertation if property types are complext or require some specific logic.
        /// If it is null the system will try to convert it via standard way
        /// </summary>
        public ActionHandler<object, object> Converter { get; set; }
    }
}

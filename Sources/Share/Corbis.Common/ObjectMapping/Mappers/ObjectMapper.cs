using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using Corbis.Common.ObjectMapping.Interface;

namespace Corbis.Common.ObjectMapping.Mappers
{
    /// <summary>
    /// Contains base simple object mapper implementation.
    /// It is thread safe object and it can be as singleton
    /// </summary>
    public class ObjectMapper : MapperBase
    {
        #region Instance

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ObjectMapper Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (typeof(ObjectMapper))
                    {
                        if (m_Instance == null)
                            m_Instance = new ObjectMapper();
                    }
                }
                return m_Instance;
            }
        }
        private static ObjectMapper m_Instance = null;

        #endregion

        #region Overrides of MapperBase

        /// <summary>
        /// Maps the source object to the target object. Contains the main logic of mapping.
        /// </summary>
        /// <param name="from">The source object.</param>
        /// <param name="to">The target object.</param>
        /// <param name="mappingData">The mapping data.</param>
        public override void DoMapping(object from, object to, MappingData mappingData)
        {
            if (from == null || to == null)
                throw new ArgumentNullException();

            mappingData = mappingData ?? new MappingData();

            //get target properties to copy data into them
            Dictionary<string,PropertyInfo> targetProperties = this.GetWritableProperties(to.GetType()).ToDictionary(p=>p.Name.ToLower(),p=>p);

            List<string> excludes = (mappingData.ExcludedProperties.Count == 0) ? null : mappingData.ExcludedProperties.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            foreach (PropertyInfo sourceProperty in from.GetType().GetProperties().Where(x => x.CanRead))
            {
                if (excludes != null && excludes.Count > 0)
                {
                    if (excludes.Where(x => string.Equals(x, sourceProperty.Name, mappingData.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)).SingleOrDefault() != null)
                        continue;
                }

                // Find a set of properties for current.
                List<MappingDataItem> mappingDataItems = mappingData.PropertiesMapping.Where(x => string.Equals(x.PropertyFrom, sourceProperty.Name, mappingData.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ).ToList();

                if (targetProperties.ContainsKey(sourceProperty.Name.ToLower()))
                {
                    bool bExists = mappingDataItems.Where(x => string.Equals(x.PropertyFrom, sourceProperty.Name, mappingData.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)  && string.Equals(x.PropertyTo, sourceProperty.Name, mappingData.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)).FirstOrDefault() != null;

                    if(!bExists)
                        mappingDataItems.Add(new MappingDataItem(sourceProperty.Name, sourceProperty.Name, null, mappingData.IgnoreCase));
                }

                foreach (MappingDataItem mdi in mappingDataItems)
                {
                    string mappingDataPropertyTo = mdi.PropertyTo.ToLower();

                    if (targetProperties.ContainsKey(mappingDataPropertyTo))
                    {
                        this.CopyData(from, to, sourceProperty, targetProperties[mappingDataPropertyTo], mdi.Converter);
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<PropertyInfo> GetWritableProperties(Type type)
        {
            List<PropertyInfo> output = new List<PropertyInfo>();

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.CanWrite)
                {
                    output.Add(property);
                }
                else
                {
                    if (property.PropertyType.IsInterface)
                    {
                        if (property.PropertyType.Name.Contains("IList"))
                            output.Add(property);
                    }
                    else
                    {
                        //By default collection we consider only IList implementators
                        if (property.PropertyType.GetInterfaces().Where(x => x.Name.Contains("IList")).Count() > 1)
                            output.Add(property);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">source object</param>
        /// <param name="to">target object</param>
        /// <param name="sourceProperty">daua supplier property</param>
        /// <param name="property">target property</param>
        /// <param name="converter">data converter</param>
        protected virtual void CopyData(object from, object to, PropertyInfo sourceProperty, PropertyInfo property, ActionHandler<object, object> converter = null)
        {
            try
            {
                object fromDataItem = sourceProperty.GetValue(from, null);

                if (property.CanWrite)
                {
                    bool bSkip = false;

                    if (converter != null)
                    {
                        fromDataItem = converter(fromDataItem);
                    }
                    else
                    {
                        Type typeFrom = sourceProperty.PropertyType;
                        Type typeTo = property.PropertyType;

                        if (!(typeTo == typeFrom || typeTo.IsAssignableFrom(typeFrom)))
                        {
                            bool bProcecced = false;

                            if (typeTo.IsValueType && typeFrom.IsValueType)
                            {
                                if ((typeTo.IsGenericType && typeTo.Name == "Nullable`1") && !typeFrom.IsGenericType)
                                {
                                    try
                                    {
                                        Type type = typeTo.GetGenericArguments()[0];

                                        object fromOut;
                                        fromDataItem = TryConvertFromMetadata(fromDataItem, type, out fromOut) ? fromOut : Convert.ChangeType(fromDataItem, type);
                                        bProcecced = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex);
                                    }
                                }
                                else if (!typeTo.IsGenericType && (typeFrom.IsGenericType && typeFrom.Name == "Nullable`1"))
                                {
                                    bSkip = fromDataItem == null;
                                    bProcecced = true;
                                }
                                else if ((typeTo.IsGenericType && typeTo.Name == "Nullable`1") && (typeFrom.IsGenericType && typeFrom.Name == "Nullable`1"))
                                {
                                    if (fromDataItem != null)
                                    {
                                        Type tfrom = typeFrom.GetGenericArguments()[0];
                                        Type tto = typeTo.GetGenericArguments()[0];

                                        if (tto == tfrom || tto.IsAssignableFrom(tfrom))
                                        {
                                            fromDataItem = Activator.CreateInstance(typeTo, fromDataItem);
                                            bProcecced = true;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                object fromOut;
                                                fromDataItem = TryConvertFromMetadata(fromDataItem, tto, out fromOut) ? fromOut : Convert.ChangeType(fromDataItem, tto);
                                                bProcecced = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex);
                                            }
                                        }
                                    }
                                }
                            }

                            if (!bProcecced)
                            {
                                try
                                {
                                    object fromOut;
                                    fromDataItem = TryConvertFromMetadata(fromDataItem, typeTo, out fromOut) ? fromOut : Convert.ChangeType(fromDataItem, typeTo);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                }
                            }
                        }
                    }

                    if (!bSkip)
                        property.SetValue(to, fromDataItem, null);
                }
                else
                {
                    //By default collection we consider only IList implementators.
                    IList propertyValue = property.GetValue(to, null) as IList;

                    if (propertyValue != null)
                    {
                        propertyValue.Clear();

                        IEnumerable items = sourceProperty.GetValue(from, null) as IEnumerable;

                        if (items != null)
                        {
                            if (converter == null)
                            {
                                if (property.PropertyType.IsGenericType && sourceProperty.PropertyType.IsGenericType)
                                {
                                    Type typeTo = property.PropertyType.GetGenericArguments()[0];
                                    Type typeFrom = sourceProperty.PropertyType.GetGenericArguments()[0];

                                    bool bConvert = !(typeFrom == typeTo || typeTo.IsAssignableFrom(typeFrom));

                                    foreach (object item in items)
                                        propertyValue.Add(bConvert ? Convert.ChangeType(item, typeTo) : item);
                                }
                                else
                                {
                                    foreach (object item in items)
                                        propertyValue.Add(item);
                                }
                            }
                            else
                            {
                                foreach (object item in items)
                                    propertyValue.Add(converter(item));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion
    }
}

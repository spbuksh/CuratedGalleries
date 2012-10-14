using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Corbis.Common.ObjectMapping.Interface;

namespace Corbis.Common.ObjectMapping.Mappers
{
    /// <summary>
    /// A base class for object mapper functionality.
    /// </summary>
    public abstract class MapperBase : IMapper
    {
        #region DoMapping

        /// <summary>
        /// Maps the source object to a new object of the target type.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="from">The source object.</param>
        /// <returns>The target object.</returns>
        public virtual T DoMapping<T>(object from)
        {
            if (object.ReferenceEquals(from, null))
                return default(T);

            return DoMapping<T>(from, null);
        }

        /// <summary>
        /// Maps the source object to a new object of the target type.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="from">The source object.</param>
        /// <param name="mappingData">The mappng data.</param>
        /// <returns>The target object.</returns>
        public virtual T DoMapping<T>(object from, MappingData mappingData)
        {
            if (object.ReferenceEquals(from, null))
                return default(T);

            T to;

            if (TryConvertFromMetadata<T>(from, out to))
                return to;

            to = Activator.CreateInstance<T>();
            DoMapping<T>(from, to, mappingData);

            return to;
        }

        /// <summary>
        /// Maps the source object to the target object.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="from">The source object.</param>
        /// <param name="to">The target object.</param>
        /// <param name="mappingData">The mapping data.</param>
        public virtual void DoMapping<T>(object from, T to, MappingData mappingData)
        {
            this.DoMapping(from, (object)to, mappingData);
        }

        /// <summary>
        /// Maps the source object to the target object.
        /// </summary>
        /// <typeparam name="T">The type of the target object.</typeparam>
        /// <param name="from">The source object.</param>
        /// <param name="to">The target object.</param>
        public virtual void DoMapping<T>(object from, T to)
        {
            this.DoMapping(from, (object)to, null);
        }

        /// <summary>
        /// Maps the source object to the target object. Contains the main logic of mapping.
        /// </summary>
        /// <param name="from">The source object.</param>
        /// <param name="to">The target object.</param>
        /// <param name="mappingData">The mapping data.</param>
        public abstract void DoMapping(object from, object to, MappingData mappingData);

        #endregion

        #region Helpers

        /// <summary>
        /// Gets the converters from the metadata.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="output">The output.</param>
        protected void GetConvertersFromMetadata(IEnumerable<Type> types, List<TypeConverterAttribute> output)
        {
            Type attrType = typeof(TypeConverterAttribute);

            foreach (Type item in types)
                output.AddRange(item.GetCustomAttributes(attrType, false).Cast<TypeConverterAttribute>());
        }

        /// <summary>
        /// Gets the converters from metadata.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>The converters.</returns>
        protected List<TypeConverterAttribute> GetConvertersFromMetadata(IEnumerable<Type> types)
        {
            List<TypeConverterAttribute> output = new List<TypeConverterAttribute>();
            GetConvertersFromMetadata(types, output);
            return output;
        }

        /// <summary>
        /// Tries to convert the source object to the expected type.
        /// </summary>
        /// <param name="from">The source object.</param>
        /// <param name="typeTo">The type to convert the source object.</param>
        /// <param name="output">The converted object.</param>
        /// <returns><c>true</c> if the source object can be converted to the expected type; otherwise, <c>false</c>.</returns>
        protected bool TryConvertFromMetadata(object from, Type typeTo, out object output)
        {
            output = null;

            if (ReferenceEquals(from, null))
                return false;

            foreach (TypeConverterAttribute item in GetConvertersFromMetadata(new Type[] { from.GetType(), typeTo }))
            {
                TypeConverter converter = Activator.CreateInstance(Type.GetType(item.ConverterTypeName)) as TypeConverter;

                if (converter.CanConvertTo(typeTo))
                {
                    output = converter.ConvertTo(from, typeTo);
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Tries to convert the source object to the expected type. If the object cannot be converted, returns the default value of the expected type.
        /// </summary>
        /// <typeparam name="T">The type to convert the source object.</typeparam>
        /// <param name="from">The source object.</param>
        /// <param name="to">The converted object.</param>
        /// <returns><c>true</c> if the source object can be converted to the expected type; otherwise, <c>false</c>.</returns>
        protected bool TryConvertFromMetadata<T>(object from, out T to)
        {
            object output;

            if (TryConvertFromMetadata(from, typeof(T), out output))
            {
                to = (T)output;
                return true;
            }

            to = default(T);
            return false;
        }

        #endregion
    }
}

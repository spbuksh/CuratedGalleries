using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.Public.Entity
{
    [Serializable, DataContract]
    public class Size<T>
        where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Height
        /// </summary>
        [DataMember]
        public T Height { get; set; }

        /// <summary>
        /// Width
        /// </summary>
        [DataMember]
        public T Width { get; set; }
    }
}

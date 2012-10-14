using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Public.Entity
{
    /// <summary>
    /// Represents range of values of specific type
    /// </summary>
    /// <typeparam name="T">Values type</typeparam>
    [Serializable]
    public class Range<T>
    {
        public virtual T From { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual T To { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Public.Entity
{
    /// <summary>
    /// Application scope
    /// </summary>
    [Flags, Serializable]
    public enum Scope
    {
        /// <summary>
        /// Search application scope
        /// </summary>
        Search = 1,
        /// <summary>
        /// LightBox application scope
        /// </summary>
        LightBox = 2,
        /// <summary>
        /// All application scopes
        /// </summary>
        All = Scope.Search | Scope.LightBox
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common
{
    /// <summary>
    /// This delegate provide logic of object receiving. For example it can be "factory" logic for object creation.
    /// Also it can be simple object deligation
    /// </summary>
    public delegate T ProviderHandler<T>();

    /// <summary>
    /// This interface provide logic of object receiving. For example it can be "factory" logic for object creation.
    /// Also it can be simple object deligation
    /// </summary>
    public interface IProvider<T>
    {
        T GetInstance();
    }
}

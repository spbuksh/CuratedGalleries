using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Corbis.Common
{
    public static class SingletonProvider<T> where T : class
    {
        private static ActionHandler<T> Factory { get; set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Initialize(T instance)
        {
            m_Instance = instance;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Initialize(ActionHandler<T> factory)
        {
            Factory = factory;
        }

        public static T Instance
        {
            get 
            {
                if (m_Instance == null)
                {
                    lock (typeof(SingletonProvider<T>))
                    {
                        if (m_Instance == null)
                            m_Instance = Factory == null ? null : Factory();
                    }
                }
                return m_Instance;
            }
        }
        private static T m_Instance = null;
    }
}

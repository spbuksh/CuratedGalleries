using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corbis.CMS.Entity;
using System.IO;

namespace Corbis.CMS.Web.Code
{
    public static partial class Extensions
    {
        public class PredicateEqualityComparer<T> : EqualityComparer<T>
        {
            private Func<T, T, bool> predicate;

            public PredicateEqualityComparer(Func<T, T, bool> predicate)
                : base()
            {
                this.predicate = predicate;
            }

            public override bool Equals(T x, T y)
            {
                if (x != null)
                {
                    return ((y != null) && this.predicate(x, y));
                }

                if (y != null)
                {
                    return false;
                }

                return true;
            }

            public override int GetHashCode(T obj)
            {
                // Always return the same value to force the call to IEqualityComparer<T>.Equals
                return 0;
            }

           
        }
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> predicate)
        {
            return source.Distinct(new PredicateEqualityComparer<TSource>(predicate));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hao.Utility
{
    /// <summary>
    /// IEnumerable扩展
    /// </summary>
    public static class IEnumerableExtension
    {

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<bool> condition, Func<TSource, bool> predicate)
        {
            if (!condition())
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<Func<TSource, bool>, Func<TSource, bool>> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate((TSource d) => true));
        }


        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<Func<TSource, bool>> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate());
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<bool> condition, Func<TSource, int, bool> predicate)
        {
            if (!condition())
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<Func<TSource, int, bool>, Func<TSource, int, bool>> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate((TSource d, int i) => true));
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<Func<TSource, int, bool>> predicate)
        {
            if (!condition)
            {
                return source;
            }
            return source.Where(predicate());
        }
    
   
        public static IEnumerable<TSource> With<TSource>(this IEnumerable<TSource> source, Action<TSource, int> withor)
        {
            if (source == null || withor == null)
            {
                yield break;
            }
            int index = -1;
            foreach (TSource tsource in source)
            {
                int num = index;
                index = checked(num + 1);
                withor(tsource, index);
                yield return tsource;
            }
            yield break;
        }


        public static bool HasValue<TSource>(this IEnumerable<TSource> thisValue)
        {
            if (thisValue == null || thisValue.Count() == 0) return false;
            return true;
        }
    }
}


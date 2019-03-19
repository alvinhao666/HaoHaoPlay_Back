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
        /// <summary>
        /// 当指定的值不为空时，执行筛选条件
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TStruct"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource, TStruct>(this IEnumerable<TSource> source, TStruct? condition, Func<TSource, bool> predicate) where TStruct : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource, TStruct>(this IEnumerable<TSource> source, TStruct? condition, Func<Func<TSource, bool>, Func<TSource, bool>> predicate) where TStruct : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate((TSource d) => true));
        }

        public static IEnumerable<TSource> WhereIf<TSource, TStruct>(this IEnumerable<TSource> source, TStruct? condition, Func<Func<TSource, bool>> predicate) where TStruct : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate());
        }

        /// <summary>
        /// 当指定的值不为空时，执行筛选条件
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, string condition, Func<TSource, bool> predicate)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, Func<bool> condition, Func<TSource, bool> predicate)
        {
            if (!condition())
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, string condition, Func<Func<TSource, bool>, Func<TSource, bool>> predicate)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return source;
            }
            return source.Where(predicate((TSource d) => true));
        }

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, string condition, Func<Func<TSource, bool>> predicate)
        {
            if (string.IsNullOrEmpty(condition))
            {
                return source;
            }
            return source.Where(predicate());
        }

        /// <summary>
        /// 当指定的值不为空时，执行筛选条件
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 当指定的值不为空时，执行筛选条件
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 当指定的值不为空时，执行筛选条件
        /// </summary>
        /// <typeparam name="TSource1"></typeparam>
        /// <typeparam name="TSource2"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource1> WhereIf<TSource1, TSource2>(this IEnumerable<TSource1> source, TSource2? condition, Func<TSource1, int, bool> predicate) where TSource2 : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IEnumerable<TSource1> WhereIf<TSource1, TSource2>(this IEnumerable<TSource1> source, TSource2? condition, Func<Func<TSource1, int, bool>, Func<TSource1, int, bool>> predicate) where TSource2 : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate((TSource1 d, int i) => true));
        }

        public static IEnumerable<TSource1> WhereIf<TSource1, TSource2>(this IEnumerable<TSource1> source, TSource2? condition, Func<Func<TSource1, int, bool>> predicate) where TSource2 : struct
        {
            if (condition == null)
            {
                return source;
            }
            return source.Where(predicate());
        }

    

        /// <summary>
        /// 使用指定的比较方法，获取两个集合的差集
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Except<TSource, TValue>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TValue> keySelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            return first.Except(second, new DefaultEqualityComparer<TSource, TValue>(keySelector));
        }

        /// <summary>
        /// 使用指定的比较方法，获取两个集合的差集
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Except<TSource, TValue>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TValue> keySelector, IEqualityComparer<TValue> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return first.Except(second, new DefaultEqualityComparer<TSource, TValue>(keySelector, comparer));
        }

        /// <summary>
        /// 使用指定的比较方法，获取集合中不重复的元素
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            return source.Distinct(new DefaultEqualityComparer<TSource, TValue>(keySelector));
        }

        /// <summary>
        /// 使用指定的比较方法，获取集合中不重复的元素
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> keySelector, IEqualityComparer<TValue> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return source.Distinct(new DefaultEqualityComparer<TSource, TValue>(keySelector, comparer));
        }

        /// <summary>
        /// 对集合中的元素执行委托
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
            {
                return null;
            }
            if (!(source is ICollection<TSource>))
            {
                source = source.ToArray<TSource>();
            }
            foreach (TSource obj in source)
            {
                action(obj);
            }
            return source;
        }

        /// <summary>
        /// 对指定对象执行委托
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="withor"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> With<TSource>(this IEnumerable<TSource> source, Action<TSource> withor)
        {
            return new ApplyIterator<TSource>(source, withor);
        }

        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null || action == null)
            {
                return null;
            }
            if (!(source is ICollection<TSource>))
            {
                source = source.ToArray<TSource>();
            }
            int num = -1;
            checked
            {
                foreach (TSource arg in source)
                {
                    num++;
                    action(arg, num);
                }
                return source;
            }
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
            IEnumerator<TSource> enumerator = null;
            yield break;
        }


    }
}


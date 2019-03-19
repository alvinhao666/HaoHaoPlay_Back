using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    internal class DefaultEqualityComparer<T, V> : IEqualityComparer<T>
    {

        public DefaultEqualityComparer(Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            this._keySelector = keySelector;
            this._comparer = comparer;
        }


        public DefaultEqualityComparer(Func<T, V> keySelector) : this(keySelector, EqualityComparer<V>.Default)
        {
        }


        public virtual bool Equals(T x, T y)
        {
            return this._comparer.Equals(this._keySelector(x), this._keySelector(y));
        }

        public virtual int GetHashCode(T obj)
        {
            return this._comparer.GetHashCode(this._keySelector(obj));
        }


        protected Func<T, V> _keySelector;


        protected IEqualityComparer<V> _comparer;
    }
}

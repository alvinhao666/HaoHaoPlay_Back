using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    internal class ApplyIterator<TSource> : IEnumerable<TSource>, IEnumerable, IEnumerator<TSource>, IDisposable, IEnumerator
    {

        public ApplyIterator(IEnumerable<TSource> source, Action<TSource> selector)
        {
            this._threadId = Environment.CurrentManagedThreadId;
            this._source = source;
            this._selector = selector;
        }


        public virtual TSource Current
        {
            get
            {
                return this._current;
            }
        }


        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public virtual void Dispose()
        {
            this._current = default(TSource);
            this._state = -1;
        }


        public virtual ApplyIterator<TSource> Clone()
        {
            return new ApplyIterator<TSource>(this._source, this._selector);
        }


        public virtual IEnumerator<TSource> GetEnumerator()
        {
            var applyIterator = (this._state == 0 && this._threadId == Environment.CurrentManagedThreadId) ? this : this.Clone();
            applyIterator._state = 1;
            return applyIterator;
        }
        
        public virtual bool MoveNext()
        {
            if (this._state == 1)
            {
                this._enumerator = this._source.GetEnumerator();
                this._state = 2;
            }
            if (this._state == 2)
            {
                if (this._enumerator.MoveNext())
                {
                    this._selector(this._current = this._enumerator.Current);
                    return true;
                }
                this.Dispose();
            }
            return false;
        }

        public virtual void Reset()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        protected readonly IEnumerable<TSource> _source;


        protected readonly Action<TSource> _selector;


        protected readonly int _threadId;


        internal int _state;


        internal TSource _current;


        protected IEnumerator<TSource> _enumerator;
    }
}

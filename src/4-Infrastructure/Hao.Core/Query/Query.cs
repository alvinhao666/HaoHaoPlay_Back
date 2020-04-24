using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.Core
{
    public abstract class Query<T> : IQuery<T> where T : new()
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 适用于单表
        /// </summary>
        public abstract List<Expression<Func<T, bool>>> QueryExpressions { get; }

        /// <summary>
        /// 适用于多表
        /// </summary>
        public virtual string QuerySql { get; }

        public virtual string OrderFileds { get; set; }

        //public OrderByType? OrderByType { get; set; }
    }
}

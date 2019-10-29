using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Hao.Core.Model;
using SqlSugar;

namespace Hao.Core.Query
{
    public abstract class Query<T> : IQuery
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 适用于单表
        /// </summary>
        public abstract List<IConditionalModel> Conditions { get; }
        public virtual string OrderFileds { get ; set ; }
        /// <summary>
        /// 适用于多表
        /// </summary>
        public virtual string QuerySql { get; }

        //public virtual Expression<Func<T, object>> Expression { get; set; }
        
        public  OrderByType? OrderByType { get; set; }
    }
}

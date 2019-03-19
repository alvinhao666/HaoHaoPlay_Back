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
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public abstract List<IConditionalModel> Conditions { get; }
        public virtual string OrderFileds { get ; set ; }

        //public virtual Expression<Func<T, object>> Expression { get; set; }
        //public virtual OrderByType OrderByType { get; set; } = OrderByType.Asc;
    }
}

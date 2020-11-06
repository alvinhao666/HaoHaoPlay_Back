using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.Core
{
    public interface IQuery<T> : IPagedQuery where T : new()
    {
        List<Expression<Func<T, bool>>> QueryExpressions { get; }

        string OrderByFileds { get; set; }
    }
}

using Hao.Query;
using SqlSugar;
using System.Collections.Generic;

namespace Hao.Core.Query
{
    public interface IQuery:IPagedQuery
    {
        List<IConditionalModel> Conditions { get; }

        string OrderFileds { get; set; }

        //Expression<Func<T, object>> Expression { get; set; }
        OrderByType? OrderByType { get; set; } 
    }
}

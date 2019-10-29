using Hao.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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

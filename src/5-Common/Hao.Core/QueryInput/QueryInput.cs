using Hao.Core.Query;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.QueryInput
{
    public class QueryInput : IPagedQuery,IQueryInput
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        //public string OrderFileds { get; set; }

        public OrderByType OrderByType { get; set; }
    }
}

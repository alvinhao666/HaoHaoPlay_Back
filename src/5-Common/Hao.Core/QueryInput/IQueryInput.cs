using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.QueryInput
{
    public interface IQueryInput
    {
        string OrderFileds { get; set; }

        //OrderByType OrderByType { get; set; }
    }
}

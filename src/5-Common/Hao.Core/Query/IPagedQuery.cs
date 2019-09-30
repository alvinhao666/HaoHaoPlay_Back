using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Query
{
    public interface IPagedQuery
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}

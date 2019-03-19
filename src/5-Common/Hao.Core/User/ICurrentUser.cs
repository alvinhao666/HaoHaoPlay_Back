using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public interface ICurrentUser
    {
        long? UserID { get; set; }

        string UserName { get; set; }
    }
}

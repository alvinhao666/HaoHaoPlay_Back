using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IRoleService
    {
        Task UpdateAuth(SysRole role);
    }
}

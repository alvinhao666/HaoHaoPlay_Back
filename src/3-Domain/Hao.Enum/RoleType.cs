using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Enum
{
    public enum RoleType
    {
        [HDescription("超级管理员")]
        SuperAdministrator,
        [HDescription("管理员")]
        Administrator,
        [HDescription("普通用户")]
        User
    }
}

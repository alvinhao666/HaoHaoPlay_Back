using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Enum
{
    public enum RoleType
    {
        [H_Description("超级管理员")]
        SuperAdministrator,
        [H_Description("管理员")]
        Administrator,
        [H_Description("普通用户")]
        User
    }
}

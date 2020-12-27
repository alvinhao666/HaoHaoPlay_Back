using Hao.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hao.Utility;

namespace Hao.Model
{
    /// <summary>
    /// 登录查询
    /// </summary>
    public class LoginQuery : Query<SysUser>
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }


        public override List<Expression<Func<SysUser, bool>>> QueryExpressions
        {
            get
            {
                List<Expression<Func<SysUser, bool>>> expressions = new List<Expression<Func<SysUser, bool>>>();

                if (LoginName.HasValue()) expressions.Add(x => x.LoginName == LoginName);

                if (LoginName.HasValue()) expressions.Add(x => x.Password == Password);

                return expressions;
            }
        }
    }
}

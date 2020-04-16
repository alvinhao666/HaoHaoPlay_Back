using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hao.AppService
{
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
                if (!string.IsNullOrWhiteSpace(LoginName)) expressions.Add(x => x.LoginName == LoginName);
                if (!string.IsNullOrWhiteSpace(Password)) expressions.Add(x => x.Password == Password);
                return expressions;
            }
        }
    }
}

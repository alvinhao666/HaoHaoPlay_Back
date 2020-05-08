using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Hao.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.AppService
{
    public class UserQuery : Query<SysUser>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTimeStart { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTimeEnd { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public long? RoleId { get; set; }

        /// <summary>
        /// 当前用户角色等级
        /// </summary>
        public int? CurrentRoleLevel { get; set; }

        public override List<Expression<Func<SysUser, bool>>> QueryExpressions
        {
            get
            {
                List<Expression<Func<SysUser, bool>>> expressions = new List<Expression<Func<SysUser, bool>>>();

                if (LoginName.HasValue()) expressions.Add(x => x.LoginName == LoginName);

                if (Password.HasValue()) expressions.Add(x => x.Password == Password);

                if (Name.HasValue()) expressions.Add(x => x.Name.Contains(Name));

                if (Phone.HasValue()) expressions.Add(x => x.Phone.Contains(Phone));

                if (Gender.HasValue) expressions.Add(x => x.Gender == Gender);

                if (Enabled.HasValue) expressions.Add(x => x.Enabled == Enabled);

                if (LastLoginTimeStart.HasValue) expressions.Add(x => x.LastLoginTime >= LastLoginTimeStart);

                if (LastLoginTimeEnd.HasValue) expressions.Add(x => x.LastLoginTime <= LastLoginTimeEnd);

                if (RoleId.HasValue) expressions.Add(x => x.RoleId == RoleId);

                if (CurrentRoleLevel.HasValue) expressions.Add(x => x.RoleLevel > CurrentRoleLevel);

                return expressions;
            }
        }

        //public override string QuerySql
        //{
        //    get
        //    {
        //        StringBuilder sb = new StringBuilder(" 1=1 ");


        //        if (!string.IsNullOrWhiteSpace(Code))
        //            sb.Append($" And st.Code like '%{Code}%'");

        //        if (!string.IsNullOrWhiteSpace(Name))
        //            sb.Append($" And st.Name like '%{Name}%'");

        //        if (!string.IsNullOrWhiteSpace(Gender))
        //            sb.Append($" And st.Gender like '%{Gender}%'");

        //        if (!string.IsNullOrWhiteSpace(Mobile))
        //            sb.Append($" And st.Mobile like'%{Mobile}%'");

        //        if (Sta.HasValue)
        //            sb.Append($" And st.Status={(int)Sta}");


        //        return sb.ToString();
        //    }
        //}
    }
}

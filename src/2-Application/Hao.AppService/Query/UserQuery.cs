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
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public Gender? Gender { get; set; }

        public string Phone { get; set; }

        public bool? Enabled { get; set; } 

        public DateTime? LastLoginTimeStart { get; set; }

        public DateTime? LastLoginTimeEnd { get; set; }

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
                if (Gender.HasValue) expressions.Add(x => x.Gender==Gender);
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

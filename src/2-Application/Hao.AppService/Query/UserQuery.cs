using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Hao.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

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


        public override List<IConditionalModel> Conditions
        {
            get
            {
                List<IConditionalModel> models = new List<IConditionalModel>();

                if (!string.IsNullOrWhiteSpace(LoginName))
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.LoginName), ConditionalType = ConditionalType.Equal, FieldValue = LoginName });

                if (!string.IsNullOrWhiteSpace(Password))
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.Password), ConditionalType = ConditionalType.Equal, FieldValue = Password });

                if (!string.IsNullOrWhiteSpace(Name))
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.Name), ConditionalType = ConditionalType.Like, FieldValue = Name });

                if(!string.IsNullOrWhiteSpace(Phone))
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.Phone), ConditionalType = ConditionalType.Like, FieldValue = Phone });

                if (Gender.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.Gender), ConditionalType = ConditionalType.Equal, FieldValue = ((int)Gender).ToString() });

                if (Enabled.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.Enabled), ConditionalType = ConditionalType.Equal, FieldValue = Convert.ToInt32(Enabled).ToString() });

                if (LastLoginTimeStart.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.LastLoginTime), ConditionalType = ConditionalType.GreaterThanOrEqual, FieldValue = LastLoginTimeStart.ToDateString() });

                if (LastLoginTimeEnd.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SysUser.LastLoginTime), ConditionalType = ConditionalType.LessThanOrEqual, FieldValue = LastLoginTimeEnd.ToDateString() });

                return models;

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

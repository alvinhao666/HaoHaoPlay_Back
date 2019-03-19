using Hao.Core.Query;
using Hao.Model;
using Hao.Model.Enum;
using Hao.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    public class UserQuery : Query<SYSUser>
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

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
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.LoginName), ConditionalType = ConditionalType.Equal, FieldValue = LoginName });

                if (!string.IsNullOrWhiteSpace(Password))
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.Password), ConditionalType = ConditionalType.Equal, FieldValue = Password });

                if (!string.IsNullOrWhiteSpace(UserName))
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.UserName), ConditionalType = ConditionalType.Like, FieldValue = UserName });

                if(!string.IsNullOrWhiteSpace(Phone))
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.Phone), ConditionalType = ConditionalType.Like, FieldValue = Phone });

                if (Gender.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.Gender), ConditionalType = ConditionalType.Equal, FieldValue = ((int)Gender).ToString() });

                if (Enabled.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.Enabled), ConditionalType = ConditionalType.Equal, FieldValue = Convert.ToInt32(Enabled).ToString() });

                if (LastLoginTimeStart.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.LastLoginTime), ConditionalType = ConditionalType.GreaterThanOrEqual, FieldValue = LastLoginTimeStart.Value.ToDateString() });

                if (LastLoginTimeEnd.HasValue)
                    models.Add(new ConditionalModel() { FieldName = nameof(SYSUser.LastLoginTime), ConditionalType = ConditionalType.LessThanOrEqual, FieldValue = LastLoginTimeEnd.Value.ToDateString() });

                return models;

            }
        }
    }
}

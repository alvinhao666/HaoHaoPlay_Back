using Hao.Core.QueryInput;
using Hao.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class UserQueryInput:QueryInput
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public Gender? Gender { get; set; }

        public string Phone { get; set; }

        public bool? Enabled { get; set; }

        public DateTime? LastLoginTimeStart { get; set; }

        public DateTime? LastLoginTimeEnd { get; set; }
    }
}

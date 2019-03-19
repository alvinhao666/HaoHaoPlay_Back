using Hao.Model.Enum;
using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class UserVMOut
    {
        public string ID { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }
        
        public Gender? Gender { get; set; }

        public string GenderString { get; set; }

        public int? Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WeChat { get; set; }

        public bool? Enabled { get; set; }

        public string EnabledString { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string LastLoginIP { get; set; }
    }
}

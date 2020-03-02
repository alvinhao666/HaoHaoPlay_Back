using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class UserDetailVM
    {

        public string Name { get; set; }

        public Gender? Gender { get; set; }

        public string GenderString { get; set; }

        public int? Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WeChat { get; set; }

        public bool? Enabled { get; set; }

    }
}

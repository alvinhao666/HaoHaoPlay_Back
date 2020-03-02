using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class PwdUpdateRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RePassword { get; set; }
    }
}

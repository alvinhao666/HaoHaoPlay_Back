using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    public class SysLoginRecord
    {
        public long? Id { get; set; }

        public long? UserId { get; set; }

        public string IP { get; set; }

        public DateTime? Time { get; set; }
    }
}

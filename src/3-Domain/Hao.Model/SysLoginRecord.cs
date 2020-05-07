using System;
using Hao.Core;

namespace Hao.Model
{
    public class SysLoginRecord: BaseEntity<long>
    {
        public long? UserId { get; set; }

        public string IP { get; set; }

        public DateTime? Time { get; set; }
    }
}

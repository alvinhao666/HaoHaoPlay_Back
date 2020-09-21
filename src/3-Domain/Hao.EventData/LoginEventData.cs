using System;

namespace Hao.EventData
{
    public class LoginEventData
    {
        public long? UserId { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public string LastLoginIP { get; set; }
    }
}

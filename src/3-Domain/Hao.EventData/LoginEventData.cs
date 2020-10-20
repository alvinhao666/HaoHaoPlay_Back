using System;

namespace Hao.EventData
{
    public class LoginEventData
    {
        /// <summary>
        /// 登录用户id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 登录ip
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// Json Web Token唯一标识
        /// </summary>
        public Guid? JwtJti { get; set; }

        /// <summary>
        /// Jwt过期时间
        /// </summary>
        public DateTime? JwtExpireTime { get; set; }
    }
}

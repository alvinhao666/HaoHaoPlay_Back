using System;
using Hao.Core;

namespace Hao.Model
{
    public class SysLoginRecord: BaseEntity<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 登录ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? Time { get; set; }

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

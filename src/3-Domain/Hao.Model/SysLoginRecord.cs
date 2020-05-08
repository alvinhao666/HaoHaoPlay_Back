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
    }
}

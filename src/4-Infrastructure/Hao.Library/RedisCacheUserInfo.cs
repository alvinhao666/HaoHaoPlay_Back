using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Library
{
    /// <summary>
    /// 用户认证实体
    /// </summary>
    public class RedisCacheUserInfo
    {
        public  long? Id { get; set; }

        public string LoginName { get; set; }

        public string Name { get; set; }
    }
}

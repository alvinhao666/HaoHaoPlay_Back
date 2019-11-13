using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Library
{
    public class JwtOptions
    {
        public SigningCredentials SigningKey { get; set; }

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }


        public string SecretKey { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
    }
}

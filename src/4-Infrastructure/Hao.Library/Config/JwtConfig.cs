using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hao.Library
{
    public class JwtConfig
    {
        public SigningCredentials SigningCredentials
        {
            get
            {
                return this.SecurityKey == null ? null : new SigningCredentials(this.SecurityKey, SecurityAlgorithms.HmacSha256);
            }
        }

        public SecurityKey SecurityKey
        {
            get
            {
                return this.SecretKey == null ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.SecretKey));
            }
        }

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接受者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 安全密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
    }
}

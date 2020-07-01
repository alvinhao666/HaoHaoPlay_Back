namespace Hao.Library
{
    public class JwtConfig
    {
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

namespace Hao.TencentCloud.Cos
{
    public class FederationTokenConfig
    {
        /// <summary>
        /// SecretId
        /// </summary>
        public string SecretId { get; set; }
        
        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }
        
        /// <summary>
        /// 接入点
        /// </summary>
        public string EndPoint { get; set; }
        
        /// <summary>
        /// 区域
        /// </summary>
        public string Region { get; set; }
        
        /// <summary>
        /// 您可以自定义调用方英文名称，由字母组成
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 授予该临时证书权限的CAM策略
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// 指定临时证书的有效期
        /// </summary>
        public ulong? DurationSeconds { get; set; }
    }
}
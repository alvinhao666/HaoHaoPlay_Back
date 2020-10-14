namespace Hao.TencentCloud.Cos
{
    /// <summary>
    /// 获取联合身份临时访问凭证 配置项
    /// </summary>
    public class TencentCosConfig
    {
        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 对象在存储桶中的位置标识符，即称对象键
        /// </summary>
        public string UploadKey { get; set; }

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
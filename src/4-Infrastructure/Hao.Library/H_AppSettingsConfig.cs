using System.Collections.Generic;

namespace Hao.Library
{
    public class H_AppSettingsConfig
    {
        /// <summary>
        /// 数据库连接字符信息
        /// </summary>
        public ConnectionStringConfig ConnectionString { get; set; }

        /// <summary>
        /// Json Web Token信息
        /// </summary>
        public JwtConfig Jwt { get; set; }

        /// <summary>
        /// Redis前缀信息
        /// </summary>
        public RedisPrefixConfig RedisPrefix { get; set; }

        /// <summary>
        /// 雪花id信息
        /// </summary>
        public SnowflakeIdConfig SnowflakeId { get; set; }

        /// <summary>
        /// Rabbitmq配置
        /// </summary>
        public RabbitMQConfig RabbitMQ { get; set; }

        /// <summary>
        /// 密钥信息
        /// </summary>
        public KeyConfig Key { get; set; }

        /// <summary>
        /// 数据保护类别
        /// </summary>
        public DataProtectorPurposeConfig DataProtectorPurpose { get; set; }

        /// <summary>
        /// Swagger信息
        /// </summary>
        public SwaggerConfig Swagger { get; set; }

        /// <summary>
        /// 跨域地址
        /// </summary>
        public string[] CorsUrls { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public FilePathConfig FilePath { get; set; }


        /// <summary>
        /// automaper需要注入的类所在程序集名称
        /// </summary>
        public List<string> AutoMapperAssemblyNames { get; set; }

        /// <summary>
        /// 事件订阅需要注入的类所在程序集名称
        /// </summary>
        public List<string> EventSubscribeAssemblyNames { get; set; }

        /// <summary>
        /// 请求模型验证需要注入的类所在程序集名称
        /// </summary>
        public List<string> ValidatorAssemblyNames { get; set; }

        /// <summary>
        /// 依赖注入接口所在的程序集名称
        /// </summary>
        public List<string> DiAssemblyNames { get; set; }

        /// <summary>
        /// controller类所在的程序集名称
        /// </summary>
        public List<string> ControllerAssemblyNames { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public RequestPathConfig RequestPath { get; set; }
    }
}

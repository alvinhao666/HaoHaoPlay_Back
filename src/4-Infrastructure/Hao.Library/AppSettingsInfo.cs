using System.Collections.Generic;

namespace Hao.Library
{
    public class AppSettingsInfo
    {
        public ConnectionString ConnectionString { get; set; }

        public Jwt Jwt { get; set; }

        public RedisPrefix RedisPrefix { get; set; }

        public SnowflakeId SnowflakeId { get; set; }

        public RabbitMQ RabbitMQ { get; set; }

        public Key Key { get; set; }

        public DataProtectorPurpose DataProtectorPurpose { get; set; }

        public Swagger Swagger { get; set; }

        /// <summary>
        /// 跨域地址
        /// </summary>
        public string[] CorsUrls { get; set; }


        public FilePath FilePath { get; set; }


        /// <summary>
        /// automaper需要注入得类所在程序集名称
        /// </summary>
        public List<string> AutoMapperAssemblyNames { get; set; }

        /// <summary>
        /// 事件订阅需要注入得类所在程序集名称
        /// </summary>
        public List<string> EventSubscribeAssemblyNames { get; set; }

        /// <summary>
        /// 请求模型验证需要注入得类所在程序集名称
        /// </summary>
        public List<string> ValidatorAssemblyNames { get; set; }


        public List<string> IocAssemblyNames { get; set; }


        public List<string> ControllerAssemblyNames { get; set; }

        /// <summary>
        /// 服务启动地址
        /// </summary>
        public string ServiceStartUrl { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public RequestPath RequestPath { get; set; }
    }
}

namespace Hao.Library
{
    public class AppSettingsInfo
    {
        public ConnectionString ConnectionString { get; set; }

        public Jwt JwtOptions { get; set; }

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
    }
}

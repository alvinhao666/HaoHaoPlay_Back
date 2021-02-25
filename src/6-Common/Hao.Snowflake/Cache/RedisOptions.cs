namespace Hao.Snowflake.Redis
{
    public class RedisOptions: SnowflakeOptions
    {
        public int Database { get; set; }

        public string ConnectionString { get; set; }

        public string InstanceName { get; set; }
    }
}
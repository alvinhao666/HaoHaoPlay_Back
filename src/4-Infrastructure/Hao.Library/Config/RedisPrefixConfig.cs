namespace Hao.Library
{
    public class RedisPrefixConfig
    {
        /// <summary>
        /// 全局前缀
        /// </summary>
        public string GlobalKey { get; set; }

        /// <summary>
        /// 登录信息前缀
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 分布式锁前缀
        /// </summary>
        public string DistributedLock { get; set; }
    }
}
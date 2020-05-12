namespace Hao.Library
{
    public class RabbitMQ
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟消息服务器，每个VirtualHost之间是相互隔离的
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostName { get; set; }
    }
}

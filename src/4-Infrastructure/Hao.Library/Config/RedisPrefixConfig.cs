namespace Hao.Library
{
    public class RedisPrefixConfig
    {
        /// <summary>
        /// ȫ��ǰ׺
        /// </summary>
        public string GlobalKey { get; set; }

        /// <summary>
        /// ��¼��Ϣǰ׺
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// �ֲ�ʽ��ǰ׺
        /// </summary>
        public string DistributedLock { get; set; }
    }
}
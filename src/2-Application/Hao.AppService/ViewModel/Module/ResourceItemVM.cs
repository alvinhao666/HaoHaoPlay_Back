namespace Hao.AppService
{
    public class ResourceItemVM
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }
    }
}

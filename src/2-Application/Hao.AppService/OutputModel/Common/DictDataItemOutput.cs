namespace Hao.AppService
{
    /// <summary>
    /// 字典数据项
    /// </summary>
    public class DictDataItemOutput
    {
        public long? Id { get; set; }

        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数据项值
        /// </summary>
        public string ItemValue { get; set; }
    }
}

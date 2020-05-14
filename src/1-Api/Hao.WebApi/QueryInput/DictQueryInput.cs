using Hao.Core;

namespace Hao.WebApi
{
    /// <summary>
    /// 字典列表查询
    /// </summary>
    public class DictQueryInput : QueryInput
    {
        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictCode { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemName { get; set; }
    }
}
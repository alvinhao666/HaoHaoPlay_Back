using Hao.Core;

namespace Hao.AppService
{
    /// <summary>
    /// 字典列表查询
    /// </summary>
    public class DictQueryInput : QueryInput
    {
        /// <summary>
        /// 字典编码 模糊查询
        /// </summary>
        public string LikeDictCode { get; set; }

        /// <summary>
        /// 字典名称 模糊查询
        /// </summary>
        public string LikeDictName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 数据项名称 模糊查询
        /// </summary>
        public string LikeItemName { get; set; }
    }
}
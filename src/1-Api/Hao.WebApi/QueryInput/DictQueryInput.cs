using Hao.Core;

namespace Hao.WebApi
{
    public class DictQueryInput:QueryInput
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
    }
}
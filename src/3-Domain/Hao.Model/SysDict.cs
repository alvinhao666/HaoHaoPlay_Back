using Hao.Core;
using Hao.Enum;

namespace Hao.Model
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class SysDict : Entity<long>
    {
        /// <summary>
        /// 字典编号
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
        /// 数据名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        public int? ItemValue { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public DictType? DictType { get; set; }
    }
}

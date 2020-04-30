using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 字典项添加请求
    /// </summary>
    public class DictItemAddRequest
    {
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数据项值
        /// </summary>
        public int? ItemValue { get; set; }

        /// <summary>
        /// 父级 字典id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    public class DictDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long? Id { get; set; }

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
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 数据项
        /// </summary>
        public string ItemNames { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}

using System;

namespace Hao.AppService
{
    public class DictOutput
    {
        public long? Id { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }
        
        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictCode { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemNames { get; set; }
    }
}
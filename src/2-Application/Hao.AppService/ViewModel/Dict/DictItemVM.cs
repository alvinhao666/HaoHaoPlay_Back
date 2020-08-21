using System;

namespace Hao.AppService
{
    public class DictItemVM
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
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
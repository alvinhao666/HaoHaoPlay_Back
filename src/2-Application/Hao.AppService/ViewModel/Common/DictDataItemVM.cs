using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    /// <summary>
    /// 字典数据项
    /// </summary>
    public class DictDataItemVM
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

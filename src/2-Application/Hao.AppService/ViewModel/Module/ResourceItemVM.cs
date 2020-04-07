using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class ResourceItemVM
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }
    }
}

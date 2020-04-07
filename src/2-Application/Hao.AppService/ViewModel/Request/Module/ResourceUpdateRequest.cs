using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class ResourceUpdateRequest
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class ResourceAddRequest
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
    }
}

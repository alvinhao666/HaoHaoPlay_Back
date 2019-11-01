using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public class LogInfo
    {
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Argument { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

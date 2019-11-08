
using System.Text.Json;

namespace Hao.Log
{
    public class HLog
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
            return JsonSerializer.Serialize(this);
        }
    }
}

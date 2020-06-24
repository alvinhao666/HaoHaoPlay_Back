using System.Text.Encodings.Web;
using System.Text.Json;

namespace Hao.Log
{
    public class H_Log
    {
        /// <summary>
        /// 方法名
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

        /// <summary>
        /// 重写方法，必须，用于日志记录
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }
    }
}
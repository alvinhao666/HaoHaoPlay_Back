using System.Text.Encodings.Web;
using System.Text.Json;

namespace Hao.Core
{
    /// <summary>
    /// 日志
    /// </summary>
    public class H_Log
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

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
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                //在默认情况下，System.Text.Json 序列化程序对所有非 ASCII 字符进行转义；这就是中文被转义的根本原因
                //可以不必设置例外而达到不转义的效果，这个模式就是“非严格JSON”模式
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = null  //PropertyNamingPolicy = JsonNamingPolicy.CamelCase //开头字母小写 默认
            });
        }
    }
}
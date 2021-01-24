using System.Text.Encodings.Web;
using System.Text.Json;

namespace Hao.Core
{
    /// <summary>
    /// 日志信息
    /// </summary>
    public class H_Log
    {
        /// <summary>
        /// 记录当前位置，请求路径或者方法名称
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 需记录的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 其他内容描述
        /// </summary>
        public string ExtraContent { get; set; }

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

    /// <summary>
    /// 日志模板
    /// </summary>
    public static class LogTemplate
    {
        /// <summary>
        /// 默认模板
        /// </summary>
        public const string Default = "{@Log}";
    }
}
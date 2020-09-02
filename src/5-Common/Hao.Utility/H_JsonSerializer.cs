using System.Text.Encodings.Web;
using System.Text.Json;

namespace Hao.Utility
{
    public class H_JsonSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize<TValue>(TValue value)
        {
            return JsonSerializer.Serialize(value, new JsonSerializerOptions()
            {
                //在默认情况下，System.Text.Json 序列化程序对所有非 ASCII 字符进行转义；这就是中文被转义的根本原因
                //可以不必设置例外而达到不转义的效果，这个模式就是“非严格JSON”模式
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, 
                PropertyNamingPolicy = null
            });
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TValue Deserialize<TValue>(string json)
        {
            return JsonSerializer.Deserialize<TValue>(json, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = null
            });
        }
    }
}
